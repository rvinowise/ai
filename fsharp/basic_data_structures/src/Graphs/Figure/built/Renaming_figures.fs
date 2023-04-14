module rvinowise.ai.Renaming_figures
    
    open Xunit
    open FsUnit
    
    open rvinowise.ai
    open rvinowise.extensions
    open rvinowise
    open System.Collections.Generic


    
    let vertices_with_sequencial_names
        (referenced_figure: Figure_id)
        (amount:int)
        =
        List.init amount (fun number->
            (
                (Figure_id.value referenced_figure)
                +
                (string (number+1))
            )
            |>Vertex_id
        )

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

    let group_by_figures 
        (all_referenced_figures: Map<Vertex_id, Figure_id>)
        (used_vertices: Vertex_id Set)
        =
        all_referenced_figures
        |>Map.filter(fun key value -> used_vertices|>Set.contains key)
        |>extensions.Map.reverse_with_list_of_keys

    let next_vertex_id_for_figure 
        (number: int)
        (figure:Figure_id)
        =
        ((figure|>Figure_id.value)+(string number))
        |>Vertex_id

    let assign_next_numbers 
        (all_renamings: Map<Vertex_id, Vertex_id>)
        (all_figures_to_last_number: Map<Figure_id, int>)
        (figures: Map<Figure_id, Vertex_id>)
        =
        let renamings=
            figures
            |>Seq.map (fun pair->
                let figure = pair.Key
                let vertex = pair.Value
                let new_last_number = all_figures_to_last_number.TryFind(figure)|>(Option.defaultValue 0)
                
                (vertex,
                figure|>next_vertex_id_for_figure new_last_number,
                figure,
                new_last_number)
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
        (figure: Figure_id)
        (vertices: Vertex_id list)
        =
        let last_number = 
            all_figures_to_last_number
            |>Map.tryFind figure
            |>Option.defaultValue 0

        let renamings=
            figures
            |>Seq.map (fun pair->
                let figure = pair.Key
                let vertex = pair.Value
                let new_last_number = all_figures_to_last_number.TryFind(figure)|>(Option.defaultValue 0)
                
                (vertex,
                figure|>next_vertex_id_for_figure new_last_number,
                figure,
                new_last_number)
            )
        let updated_renamings =
            Seq.fold 
                (fun map (old_name,new_name,_,_) ->
                    Map.add old_name new_name map
                )
                all_renamings
                renamings
        let updated_figures_to_last_number = 
            all_figures_to_last_number
            |>Map.add 
                figure 
                (last_number+(vertices|>List.length))
        (updated_renamings,updated_figures_to_last_number)

    let sort_compeating_vertices_by_their_neighbours
        (owner_figure:Figure)
        (renamings: Map<Vertex_id, Vertex_id>)
        (vertices: Vertex_id list)
        =()

    let rec assign_numbers_to_next_vertex_wave
        (owner_figure:Figure)
        (renamings: Map<Vertex_id, Vertex_id>)
        (figures_to_last_number: Map<Figure_id, int>)
        (vertices: Vertex_id list)
        =
        let grouped_vertices = 
            vertices
            |>Set.ofList
            |>group_by_figures owner_figure.subfigures
        
        let single_figures = 
            grouped_vertices
            |>Map.filter(fun figure vertices->vertices|>Seq.length = 1)
            |>Map.map (fun figure vertex->vertex|>Seq.head)
        let (renamings, figures_to_last_number) = 
            single_figures
            |>assign_next_numbers 
                renamings 
                figures_to_last_number

        let competing_vertices =
            grouped_vertices
            |>Map.filter(fun figure vertices->vertices|>Seq.length > 1)
            |>Map.map(fun figure vertices->
                vertices
                |>sort_compeating_vertices_by_their_neighbours
                    owner_figure
                    renamings
            )

        let (renamings, figures_to_last_number) = 
            competing_vertices
            |>extensions.Map.toPairs
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

        |>Seq.map(fun pair->
            let figure = pair.Key
            let vertices = pair.Value
        )

        let next_vertices = 
            vertices
            |>Seq.collect (Edges.next_vertices owner_figure.edges)
            |>List.ofSeq
        match next_vertices with
        |[]->(renamings, figure_to_last_number)
        |next_vertices->
            assign_numbers_to_next_vertex_wave
                owner_figure
                renamings
                figure_to_last_number
                next_vertices
        
    let rename_vertices_to_standard_names 
        (owner_figure: Figure)
        =
        let figure_to_vertices = 
            owner_figure.subfigures
            |>extensions.Map.reverse_with_list_of_keys

        let first_vertices =
            owner_figure.edges
            |>Edges.first_vertices
            |>List.ofSeq
        
        let (renamings, figure_to_last_number) =
            assign_numbers_to_next_vertex_wave
                owner_figure
                Map.empty
                Map.empty
                first_vertices

        {
            edges=
                owner_figure.edges
                |>renamed_edges_for_figure vertices_to_new_names
                |>Set.ofSeq
            
            subfigures=
                owner_figure.subfigures
                |>renamed_subfigures_for_figure vertices_to_new_names
        }

    [<Fact>]
    let ``try rename_vertices_to_standard_names``()=
        built.Figure.from_tuples [
            "my_a0","a","my_b1","b";
            "my_a0","a","uppercase_b","B";
            "uppercase_b","B","c0_at_the_end","figure_c";
            "uppercase_b","B","another_a","a";
        ]|>rename_vertices_to_standard_names
        |>should equal (
            built.Figure.from_tuples [
                "a1","a","b1","b";
                "a1","a","B1","B";
                "B1","B","figure_c1","figure_c";
                "B1","B","a2","a";
            ]
        )