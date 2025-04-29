namespace rvinowise.ai

open rvinowise.ai.generating_combinations
open rvinowise.ai.stencil
open rvinowise
open rvinowise.ai.mapping_graph_impl

open Xunit
open FsUnit


module Mapping_graph_with_immutable_mapping = 

    
    (* for a figure we have a list of structures, each such structure has a vertex in the stencil,
    which can be mapped onto a sequence of vertices in the target. all those vertices reference the figure from the key *)
    let all_combinations_of_next_mappings 
        (mappings: Map<Figure_id, struct (Vertex_id*seq<Vertex_id>) list>) 
        =
        mappings
        |>Seq.map (fun pair->
            Generator_of_mappings<Vertex_id,Vertex_id> pair.Value
        )
        |>Work_with_generators.mapping_combinations_from_generators
    

    let copied_mapping_with_prolongation
        (mapping: Map<Vertex_id,Vertex_id>)
        (added_mapped_elements: seq<Element_to_target<Vertex_id, Vertex_id>>)
        =
        added_mapped_elements
        |>Seq.fold (fun mapping added_mapped_element ->
                Map.add added_mapped_element.element added_mapped_element.target mapping
            )
            mapping

    let prolongate_mapping_with_next_mapped_subfigures 
        (base_mapping: Map<Vertex_id,Vertex_id>)
        //                                                       all_vertices   combinations               
        (added_mappings: Element_to_target<Vertex_id, Vertex_id> seq            seq         )
        =
        added_mappings
        |>Seq.map (copied_mapping_with_prolongation base_mapping)

    
    let choose_first_vertices 
        (step_further: Vertex_id -> Vertex_id Set)
        (vertices: Vertex_id Set)
        =
        (*the first vertices in this group are those, which can't be reached by iterating over the graph, starting from
        any other vertex from the selected group*) 
        let reached_vertices =
            vertices
            |>Seq.fold(fun set vertex->
                    [vertex]
                    |>Search_in_graph.vertices_reacheble_from_any_vertices
                        (fun vertex->vertices|>Set.contains vertex)
                        step_further
                    |>Set.union set
                )
                Set.empty
        reached_vertices
        |>Set.difference vertices

    let first_vertices_reacheble_from_all_vertices_together
        (is_vertex_needed: Vertex_id->bool)
        (step_further: Vertex_id -> Vertex_id Set)
        (starting_vertices: Vertex_id seq)
        =
        starting_vertices
        |>Seq.map (
            Search_in_graph.vertices_reacheble_from_vertex 
                is_vertex_needed
                step_further
        )
        |>Set.intersectMany
        |>choose_first_vertices step_further

    let does_vertex_reference_figue
        owner_figure
        referenced_figure
        vertex
        =
        vertex
        |>Figure.reference_of_vertex owner_figure
            = referenced_figure

    [<Fact>]
    let ``finding following subfigures referencing a specific figure``()=
        let owner_figure = example.Figure.a_high_level_relatively_simple_figure
        let referenced_figure = (Figure_id "f")
        (first_vertices_reacheble_from_all_vertices_together 
            (does_vertex_reference_figue
                owner_figure
                referenced_figure)
            (Edges.next_vertices owner_figure.edges)
            ( "b#1"|>Vertex_id|>Set.singleton)
        )|> should equal (
            [Vertex_id "f#1";Vertex_id "f#2"]
            |>Set.ofList
        )

        (first_vertices_reacheble_from_all_vertices_together
            (does_vertex_reference_figue
                owner_figure
                referenced_figure)
            (Edges.next_vertices owner_figure.edges)
            ([Vertex_id "d#1";Vertex_id "b#2"]|>Set.ofList)
        )|> should equal (
            [Vertex_id "f#2"]
            |>Set.ofList
        )

    [<Fact>]
    let ``vertices reacheble from others``()=
        let owner_figure = example.Figure.a_high_level_relatively_simple_figure
        first_vertices_reacheble_from_all_vertices_together
            (fun _->true)
            (Edges.next_vertices owner_figure.edges)
            (["b#1";"b#2"]|>List.map Vertex_id|>Set.ofList)
        |> should equal (
            [Vertex_id "f#2"]
            |>Set.ofList
        )

    [<Fact>]
    let ``vertices reaching others``()=
        first_vertices_reacheble_from_all_vertices_together
            (fun _->true)
            (Edges.previous_vertices example.Figure.a_high_level_relatively_simple_figure.edges)
            (["b#3";"f#2"]|>List.map Vertex_id|>Set.ofList)
        |> should equal (
            [Vertex_id "b#1"]
            |>Set.ofList
        )

    

    let possible_targets_for_mapping_subfigure
        mappee
        target
        mapping
        (prolongating_stencil_subfigure: Vertex_id*Figure_id)
        =
        let prolongating_vertex = prolongating_stencil_subfigure|>fst
        let prolongating_figure = prolongating_stencil_subfigure|>snd
        
        let does_vertex_reference_needed_figure vertex =
            Figure.reference_of_vertex target vertex =
                prolongating_figure 
        
        let further_step_of_searching_targets =
            Edges.next_vertices target.edges

        prolongating_vertex
        |>Edges.previous_vertices mappee.edges
        |>Immutable_mapping.targets_of_mapping mapping
        |>first_vertices_reacheble_from_all_vertices_together
            does_vertex_reference_needed_figure
            further_step_of_searching_targets
        

    let next_mapping_targets_for_mapped_subfigures
        (possible_targets_for_mapping_vertex: Vertex_id*Figure_id -> Vertex_id Set)
        next_subfigures_to_map
        =
        let rec mapping_targets_for_next_subfigure
            (left_subfigures_to_map:  list<Vertex_id*Figure_id>)
            (found_mappings: Map<Figure_id,  struct(Vertex_id(*stencil_vertex*) * seq<Vertex_id>(*possible_targets*)) list>)
            =

            match left_subfigures_to_map with
            | [] -> found_mappings
            | current_subfigure_to_map::left_subfigures_to_map ->
                let targets = 
                    possible_targets_for_mapping_vertex
                        current_subfigure_to_map
                    
                if targets.Count = 0 then 
                    Map.empty
                else
                    let updated_mappings =
                        let figure = snd current_subfigure_to_map
                        let updated_targets_of_this_figure =
                            struct(current_subfigure_to_map|>fst, targets|>Seq.cast)
                            ::
                            (found_mappings
                            |>extensions.Map.getOrDefault figure [])
                        found_mappings
                        |>Map.add figure updated_targets_of_this_figure
                    mapping_targets_for_next_subfigure
                        left_subfigures_to_map
                        updated_mappings
        
        mapping_targets_for_next_subfigure
            (List.ofSeq next_subfigures_to_map)
            Map.empty

    let targets_for_mapping_prolongation
        (mappee:Figure)
        (target:Figure)
        (prolongated_mapping:Map<Vertex_id,Vertex_id>)
        (within_mapping:Map<Vertex_id,Vertex_id>)
        (subfigure_to_map: Vertex_id*Figure_id)
        =
        within_mapping
        |>Map.tryFind (fst subfigure_to_map)
        |>function
        |Some mapped_target_vertex ->
            Set.singleton mapped_target_vertex
        |None ->
            possible_targets_for_mapping_subfigure
                mappee
                target
                prolongated_mapping
                subfigure_to_map
    
    let prolongate_one_mapping_with_next_subfigures 
        possible_targets_for_mapping_vertex
        (next_subfigures_to_map: seq<Vertex_id*Figure_id>)
        (mapping:Map<Vertex_id,Vertex_id>)
        =
        let possible_next_mappings =
            next_mapping_targets_for_mapped_subfigures
                possible_targets_for_mapping_vertex
                next_subfigures_to_map

        if possible_next_mappings.IsEmpty then
            Seq.empty
        else
            possible_next_mappings
            |>all_combinations_of_next_mappings
            |>prolongate_mapping_with_next_mapped_subfigures mapping
    
    let rec prolongate_all_mappings 
        (mappee:Figure)
        (target:Figure)
        (last_mapped_vertices: Vertex_id seq)
        (within_mapping: Map<Vertex_id,Vertex_id>)
        (mappings: Map<Vertex_id,Vertex_id> seq)
        =
        let next_vertices_to_map = 
            last_mapped_vertices
            |>Edges.next_vertices_of_many mappee.edges

        if Seq.isEmpty next_vertices_to_map then
            mappings
        else
            let next_subfigures_to_map =
                next_vertices_to_map
                |>Figure.vertices_with_their_referenced_figures mappee
            
            mappings
            |>Seq.map (fun mapping ->
                
                let possible_targets_for_mapping_vertex =
                    targets_for_mapping_prolongation
                        mappee
                        target
                        mapping
                        within_mapping
                
                prolongate_one_mapping_with_next_subfigures 
                    possible_targets_for_mapping_vertex
                    next_subfigures_to_map
                    mapping
                    
            )
            |>Seq.collect id
            |>prolongate_all_mappings
                mappee 
                target 
                next_vertices_to_map
                within_mapping


    let map_figure_onto_target_within_mapping
        target
        mappee
        within_mapping
        =
        target
        |>Map_first_nodes.map_first_nodes_with_immutable_mapping mappee
        |>prolongate_all_mappings
            mappee 
            target
            (Figure.first_vertices mappee)
            within_mapping
            
    let map_figure_onto_target
        target
        mappee
        =
        map_figure_onto_target_within_mapping
            target
            mappee
            Map.empty