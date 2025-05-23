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
    
    
    let get_output_beginning_and_ending
        edges
        mapping
        (output_border)
        =
        output_border.before
        |>Immutable_mapping.targets_of_mapping mapping
        |>all_vertices_reacheble_from_all_vertices_together 
            (Edges.next_vertices edges)
        |>Set.ofSeq
        ,
        output_border.after
        |>Immutable_mapping.targets_of_mapping mapping
        |>all_vertices_reacheble_from_all_vertices_together 
            (Edges.previous_vertices edges)
        |>Set.ofSeq
    
    let output_vertices_from_the_middle
        (output_border: Stencil_output_border)
        (target:Figure)
        mapping
        =
        let (output_beginning,output_ending) =
            get_output_beginning_and_ending
                target.edges
                mapping
                output_border
        
        (output_beginning,output_ending)
        ||>Set.intersect 
    
    let output_vertices_from_side
        (border_vertices: Vertex_id Set)
        (step_further)
        mapping
        =
        border_vertices
        |>Immutable_mapping.targets_of_mapping mapping
        |>all_vertices_reacheble_from_all_vertices_together 
            step_further
        |>Set.ofSeq
    
    let retrieve_result_from_output_border
        (output_border: Stencil_output_border)
        (target:Figure)
        mapping 
        =
        let output_vertices =
            if output_border.before.IsEmpty && output_border.after.IsEmpty then
                failwith "output is not specified for a stencil"
            elif output_border.before.IsEmpty then
                output_vertices_from_side
                    output_border.after
                    (Edges.previous_vertices target.edges)
                    mapping
            elif output_border.after.IsEmpty then
                output_vertices_from_side
                    output_border.before
                    (Edges.next_vertices target.edges)
                    mapping
            else
                output_vertices_from_the_middle
                    output_border
                    target
                    mapping 
        
        
        if Set.isEmpty output_vertices then
            None
        else
            output_vertices
            |>built.Figure.subgraph_with_vertices target
            |>Some
            
                   

    let retrieve_result 
        stencil
        (target:Figure)
        mapping 
        =
        let (output_beginning,output_ending) =
            stencil
            |>Stencil.output
            |>fun output_node-> {
                Stencil_output_border.before =  Edges.previous_vertices stencil.edges output_node;
                after =  Edges.next_vertices stencil.edges output_node
            }
            |>get_output_beginning_and_ending
                target.edges
                mapping
                
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
        stencil
        target
        =
        stencil
        |>Figure_from_stencil.convert
        |>Mapping_graph_with_immutable_mapping.map_figure_onto_target target
        |>Seq.map (retrieve_result stencil target)
        |>Seq.choose id

    
    
    let results_of_conditional_stencil_application
        stencil
        target
        =
        stencil.figure
        |>Mapping_graph_with_immutable_mapping.map_conditional_figure_onto_target Map.empty target
        |>Seq.map (retrieve_result_from_output_border stencil.output_border target)
        |>Seq.choose id