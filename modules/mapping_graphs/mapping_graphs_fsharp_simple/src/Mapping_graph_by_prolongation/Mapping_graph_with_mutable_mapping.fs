namespace rvinowise.ai

open rvinowise.ai.generating_combinations
open rvinowise.ai.stencil
open rvinowise
open rvinowise.ai.mapping_graph_impl

open Xunit
open FsUnit


module Mapping_graph_with_mutable_mapping = 

    

    let all_combinations_of_next_mappings 
        (mappings:
            Map<
            Figure_id //the figure of next mapped vertices (they are grouped by their figures)
            ,
            struct (
                Vertex_id* // mapped vertex of the mappee (e.g. stencil)
                seq<Vertex_id> //possible target-vertices in the target-graph (e.g. figure) for mapping that vertex of the mappee
            ) list //all the next mappee's vertices (of this subfigure), which constitute the "wave" of the next mapping
            >
        ) 
        =
        mappings
        |>Seq.map (fun pair->
            Generator_of_mappings<Vertex_id,Vertex_id> pair.Value
        )
        |>Work_with_generators.mapping_combinations_from_generators
    

    let copied_mapping_with_prolongation
        mapping
        (added_mappings: seq<Element_to_target<Vertex_id, Vertex_id>>)
        =
        let mapping = Mapping.copy mapping
        added_mappings
        |>Seq.iter (fun added_mapping ->
            mapping[added_mapping.element] <- added_mapping.target
        )
        mapping

    let prolongate_mapping_with_next_mapped_subfigures 
        (base_mapping: Mapping)
        (
        added_mappings:
            Element_to_target<Vertex_id, Vertex_id> //this mappee (stencil) vertex is mapped onto this target (figure) vertex
            seq //all vertices in one mapping
            seq //all possible mappings
        )
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
        |>Seq.map Seq.singleton
        |>Seq.map (
            Search_in_graph.vertices_reacheble_from_any_vertices 
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
        let referenced_figure = Figure_id "f"|>built.Subfigure.referencing_constant_figure
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
        (prolongating_stencil_subfigure: Vertex_id*Subfigure)
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
        |>Mapping.targets_of_mapping mapping
        |>first_vertices_reacheble_from_all_vertices_together
            does_vertex_reference_needed_figure
            further_step_of_searching_targets
        

    let next_mapping_targets_for_mapped_subfigures
        mappee
        target
        base_mapping
        next_subfigures_to_map
        =
        let rec mapping_targets_for_next_subfigure
            (mappee:Figure)
            (target:Figure)
            (mapping:Mapping)
            (left_subfigures_to_map)
            //                                      stencil_vertex possible_targets
            (found_mappings: Map<Subfigure,  struct(Vertex_id   *  seq<Vertex_id>)  list>)
            =

            match left_subfigures_to_map with
            | [] -> found_mappings
            | current_subfigure_to_map::left_subfigures_to_map ->
                let targets = 
                    possible_targets_for_mapping_subfigure
                        mappee
                        target
                        base_mapping
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
                        mappee
                        target
                        mapping
                        left_subfigures_to_map
                        updated_mappings
        
        mapping_targets_for_next_subfigure
            mappee
            target
            base_mapping
            (List.ofSeq next_subfigures_to_map)
            Map.empty

    let prolongate_one_mapping_with_next_subfigures 
        (mappee:Figure)
        (target:Figure)
        (next_subfigures_to_map)
        (mapping:Mapping)
        =
        let possible_next_mappings =
            next_mapping_targets_for_mapped_subfigures
                mappee
                target
                mapping
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
        (mappings: Mapping seq)
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
            |>Seq.map (
                prolongate_one_mapping_with_next_subfigures 
                    mappee 
                    target 
                    next_subfigures_to_map
            )
            |>Seq.collect id
            |>prolongate_all_mappings
                mappee 
                target 
                next_vertices_to_map


    let map_figure_onto_target
        target
        mappee
        =
        target
        |>Map_first_nodes.map_first_nodes_with_mutable_mapping mappee
        |>prolongate_all_mappings
            mappee 
            target
            (Figure.first_vertices mappee)