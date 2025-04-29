namespace rvinowise.ai

open rvinowise.ai.stencil

module Applying_stencil = 
 
    let is_figure_without_impossible_parts 
        (impossibles: Figure seq)
        (owner_figure: Figure)
        =
        impossibles
        |>Seq.collect (Mapping_graph_with_immutable_mapping.map_figure_onto_target owner_figure)
        |>Seq.isEmpty


    let all_vertices_reacheble_from_all_vertices_together
        (step_further: Vertex_id->Vertex_id Set)
        starting_vertices
        =
        starting_vertices
        |>Seq.map Seq.singleton
        |>Seq.map (
            Search_in_graph.vertices_reacheble_from_any_vertices 
                (fun _->true)
                step_further
        )|>Set.intersectMany
            

    let retrieve_result 
        stencil
        (target:Figure)
        mapping 
        =
        let output_node = 
            stencil
            |>Stencil.output

        let output_beginning =
            output_node
            |>Edges.previous_vertices stencil.edges
            |>Immutable_mapping.targets_of_mapping mapping
            |>all_vertices_reacheble_from_all_vertices_together 
                (Edges.next_vertices target.edges)
            |>Set.ofSeq

        let output_ending =
            output_node
            |>Edges.next_vertices stencil.edges
            |>Immutable_mapping.targets_of_mapping mapping
            |>all_vertices_reacheble_from_all_vertices_together 
                (Edges.previous_vertices target.edges)
            |>Set.ofSeq
        
        let output_vertices = 
            (output_beginning,output_ending)
            ||>Set.intersect 
        
        if Set.isEmpty output_vertices then
            None
        else
            let resulting_part_of_target = 
                output_vertices
                |>built.Figure.subgraph_with_vertices target
            
            if 
                is_figure_without_impossible_parts
                    stencil.output_without 
                    resulting_part_of_target
            then
                Some resulting_part_of_target
            else
                None
            
            

    let results_of_stencil_application
        target
        stencil
        =
        stencil
        |>Figure_from_stencil.convert
        |>Mapping_graph_with_immutable_mapping.map_figure_onto_target target
        |>Seq.map (retrieve_result stencil target)
        |>Seq.choose id

    
    