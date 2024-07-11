namespace rvinowise.ai
    
    
open rvinowise.extensions
open System.Collections.Generic


module Renaming_figures =
    

    let renamed_edges_for_figure
        (old_to_new_names: Map<Vertex_id, Vertex_id>)
        (edges: Edge seq)
        =
        edges
        |>Seq.map (fun edge->
            Edge(
                old_to_new_names[edge.tail], 
                old_to_new_names[edge.head]
            )
        )

    let renamed_subfigures_for_figure 
        (old_to_new_names: Map<Vertex_id, Vertex_id>)
        (subfigures: IDictionary<Vertex_id, Figure_id>)
        =
        subfigures
        |>Seq.map(fun pair->
            old_to_new_names[pair.Key],
            pair.Value
        )|>Map.ofSeq


    type Figures_to_renamings = 
        Map<
            Figure_id, 
            (Vertex_id*Vertex_id) list
        >

    let group_by_figures //answers which vertices refer to this figure?
        (all_referenced_figures: Map<Vertex_id, Figure_id>)
        (used_vertices: Vertex_id Set)
        =
        all_referenced_figures
        |>Map.filter(fun vertex _ -> used_vertices|>Set.contains vertex)
        |>Map.reverse_with_list_of_keys

    let next_vertex_id_for_figure 
        (number: int)
        (figure:Figure_id)
        =
        ((figure|>Figure_id.value)+"#"+(string number))
        |>Vertex_id

    let vertices_with_sequencial_names
        referenced_figure
        starting_number
        amount
        =
        List.init amount (fun number->
            next_vertex_id_for_figure
                (number+starting_number)
                referenced_figure
        )

    let assign_next_numbers 
        (all_renamings: Map<Vertex_id, Vertex_id>)
        (all_figures_to_last_number: Map<Figure_id, int>)
        (figures: Map<Figure_id, Vertex_id>)
        =
        let renamings=
            figures
            |>Map.toSeq
            |>Seq.map (fun (figure,vertex)->
                let new_last_number =
                    all_figures_to_last_number.TryFind(figure)
                    |>(Option.defaultValue 0)
                    |>(+)1
                
                vertex,
                figure|>next_vertex_id_for_figure new_last_number,
                figure,
                new_last_number
            )
        let updated_renamings =
            Seq.fold 
                (fun map (old_name,new_name,_,_) ->
                    Map.add old_name new_name map
                )
                all_renamings
                renamings
        let updated_figures_to_last_number = 
            Seq.fold 
                (fun map (_,_,figure,last_number) ->
                    Map.add figure last_number map
                )
                all_figures_to_last_number
                renamings
        (updated_renamings,updated_figures_to_last_number)

    let assign_next_numbers_to_sorted_vertices
        (all_renamings: Map<Vertex_id, Vertex_id>)
        (all_figures_to_last_number: Map<Figure_id, int>)
        (referenced_figure: Figure_id)
        (vertices: Vertex_id list)
        =
        let last_number = 
            all_figures_to_last_number
            |>Map.tryFind referenced_figure
            |>Option.defaultValue 0

        let new_renamings =
            vertices
            |>List.length
            |>vertices_with_sequencial_names 
                referenced_figure
                (last_number+1)
            |>Seq.zip vertices

        new_renamings
        |>Seq.fold (fun map added->
            map|>Map.add (fst added) (snd added)
        )
            all_renamings
        ,
        all_figures_to_last_number
        |>Map.add referenced_figure (last_number+(List.length vertices))


    let form_for_comparison 
        (subfigures:Map<Vertex_id, Figure_id>)
        (renamings: Map<Vertex_id, Vertex_id>)
        (vertices: Vertex_id list)
        =
        vertices
        |>List.map(fun vertex->
            renamings
            |>Map.tryFind vertex
            |>function
            |Some renamed_vertex->renamed_vertex|>Vertex_id.value
            |None->subfigures[vertex]|>Figure_id.value
        )|>List.sort

    let compare_compeating_vertices
        (owner_figure:Figure)
        (renamings: Map<Vertex_id, Vertex_id>)
        (vertex1:Vertex_id) 
        (vertex2:Vertex_id) 
        = 
        let rec compare_neighbors
            (step_further: Vertex_id -> Vertex_id list)
            (neighbors1: Vertex_id list)
            (neighbors2: Vertex_id list)
            =
            match neighbors1,neighbors2 with
            |[],[]->0
            |_->
                let comparison = 
                    Seq.compareWith compare 
                        (neighbors1
                        |>form_for_comparison owner_figure.subfigures renamings)
                        (neighbors2
                        |>form_for_comparison owner_figure.subfigures renamings)
                    
                match comparison with 
                |0->
                    let next_neighbors1 = 
                        neighbors1
                        |>List.collect step_further
                    let next_neighbors2 = 
                        neighbors2
                        |>List.collect step_further
                    compare_neighbors 
                        step_further
                        next_neighbors1
                        next_neighbors2
                |difference->difference
        
        let comparison_of_previous = 
            compare_neighbors 
                (Edges.previous_vertices owner_figure.edges>>List.ofSeq)
                [vertex1] [vertex2]
        if comparison_of_previous<>0 then
            comparison_of_previous
        else
            compare_neighbors 
                (Edges.next_vertices owner_figure.edges>>List.ofSeq)
                [vertex1] [vertex2]


    let sort_compeating_vertices_by_their_neighbours
        (owner_figure:Figure)
        (renamings: Map<Vertex_id, Vertex_id>)
        (vertices: Vertex_id list)
        =
        vertices
        |>List.sortWith (compare_compeating_vertices owner_figure renamings)

    let vertext_hasnt_been_renamed
        (renamings: Map<Vertex_id, Vertex_id>)
        vertex
        =
        renamings
        |>Map.containsKey vertex
        |>not

    let rec assign_numbers_to_next_vertex_wave
        (owner_figure:Figure)
        (renamings: Map<Vertex_id, Vertex_id>)
        (figures_to_last_number: Map<Figure_id, int>)
        (vertices: Vertex_id list)
        =
        let grouped_vertices = 
            vertices
            |>List.filter (vertext_hasnt_been_renamed renamings)
            |>Set.ofList
            |>group_by_figures owner_figure.subfigures
        
        let vertices_of_unique_figures = 
            grouped_vertices
            |>Map.filter(fun _ vertices->vertices|>Seq.length = 1)
            |>Map.map (fun _ vertex->vertex|>Seq.head)
        let (renamings, figures_to_last_number) = 
            vertices_of_unique_figures
            |>assign_next_numbers 
                renamings 
                figures_to_last_number

        let competing_vertices =
            grouped_vertices
            |>Map.filter(fun _ vertices->vertices|>Seq.length > 1)
            |>Map.map(fun _ vertices->
                vertices
                |>sort_compeating_vertices_by_their_neighbours
                    owner_figure
                    renamings
            )

        let (renamings, figures_to_last_number) = 
            competing_vertices
            |>Map.toSeq
            |>Seq.fold (
                fun 
                    (renamings,figures_to_last_number) 
                    (figure,vertices) ->
                assign_next_numbers_to_sorted_vertices
                    renamings
                    figures_to_last_number
                    figure
                    vertices
            )
                (renamings,figures_to_last_number)


        let next_vertices = 
            vertices
            |>Seq.collect (Edges.next_vertices owner_figure.edges)
            |>List.ofSeq
        match next_vertices with
        |[]->(renamings, figures_to_last_number)
        |next_vertices->
            assign_numbers_to_next_vertex_wave
                owner_figure
                renamings
                figures_to_last_number
                next_vertices
        
    let rename_vertices_to_standard_names 
        (owner_figure: Figure)
        =
        let first_vertices =
            owner_figure
            |>Figure.first_vertices
            |>List.ofSeq
        
        let (renamings, _) =
            assign_numbers_to_next_vertex_wave
                owner_figure
                Map.empty
                Map.empty
                first_vertices

        {
            edges=
                owner_figure.edges
                |>renamed_edges_for_figure renamings
                |>Set.ofSeq
            
            subfigures=
                owner_figure.subfigures
                |>renamed_subfigures_for_figure renamings
            without=Set.empty
        }

    