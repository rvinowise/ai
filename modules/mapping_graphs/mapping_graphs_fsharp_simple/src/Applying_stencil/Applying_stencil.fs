namespace rvinowise.ai

open rvinowise.ai.stencil
open rvinowise

module Applying_stencil = 
 
    let is_figure_without_impossible_parts
        maping_id_to_function
        (impossibles: Figure<_> seq)
        (owner_figure: Figure<_>)
        =
        impossibles
        |>Seq.collect (
            Mapping_graph_with_immutable_mapping.map_figure_onto_target
                maping_id_to_function
                owner_figure
        )
        |>Seq.isEmpty


    let all_edges_reacheble_from_all_vertices_together
        (find_all_edges_in_direction)
        starting_vertices
        =
        let all_edges,all_vertices =
            starting_vertices
            |>Seq.map Seq.singleton
            |>Seq.map (
                find_all_edges_in_direction
                    (fun _->true)
            )|>extensions.Seq.unzip
        
        all_edges
        |>Set.intersectMany
        ,
        all_vertices
        |>Set.intersectMany
    
    
    let get_output_beginning_and_ending
        edges
        mapping
        (output_border)
        =
        output_border.before
        |>Immutable_mapping.targets_of_mapping mapping
        |>all_edges_reacheble_from_all_vertices_together 
            (Search_in_graph.next_parts_reacheble_from_any_vertices edges)
        ,
        output_border.after
        |>Immutable_mapping.targets_of_mapping mapping
        |>all_edges_reacheble_from_all_vertices_together 
            (Search_in_graph.previous_parts_reacheble_from_any_vertices edges)
    
        
    
    let output_vertices_from_the_middle
        (output_border: Stencil_output_border)
        (edges)
        mapping
        =
        let (output_beginning,output_ending) =
            get_output_beginning_and_ending
                edges
                mapping
                output_border
        
        let edges,vertices =
            (fst output_beginning, fst output_ending),
            (snd output_beginning, snd output_ending)
        
        Set.intersect <|| edges,
        Set.intersect <|| vertices
    
    
    let output_vertices_from_side
        (border_vertices: Vertex_id Set)
        (find_all_edges_in_direction)
        mapping
        =
        border_vertices
        |>Immutable_mapping.targets_of_mapping mapping
        |>all_edges_reacheble_from_all_vertices_together 
            find_all_edges_in_direction
    
    let retrieve_result_from_output_border
        (output_border: Stencil_output_border)
        (target:Figure<_>)
        mapping 
        =
        let output_edges,output_vertices =
            if output_border.before.IsEmpty && output_border.after.IsEmpty then
                failwith "output is not specified for a stencil"
            elif output_border.before.IsEmpty then
                output_vertices_from_side
                    output_border.after
                    (Search_in_graph.next_parts_reacheble_from_any_vertices target.edges)
                    mapping
            elif output_border.after.IsEmpty then
                output_vertices_from_side
                    output_border.before
                    (Search_in_graph.previous_parts_reacheble_from_any_vertices target.edges)
                    mapping
            else
                output_vertices_from_the_middle
                    output_border
                    target.edges
                    mapping 
        
        if Set.isEmpty output_vertices then
            None
        else
            built.Figure.from_parts_of_figure target output_vertices output_edges 
            |>Some
            
                   


    // let results_of_stencil_application
    //     stencil
    //     target
    //     =
    //     stencil
    //     |>Figure_from_stencil.convert
    //     |>Mapping_graph_with_immutable_mapping.map_figure_onto_target target
    //     |>Seq.map (retrieve_result stencil target)
    //     |>Seq.choose id

    
    
    let results_of_conditional_stencil_application
        mapping_id_to_function
        stencil
        (target: Figure<_>)
        =
        stencil.figure
        |>Mapping_graph_with_immutable_mapping.map_conditional_figure_onto_target
              mapping_id_to_function
              Map.empty
              target
        |>Seq.map (retrieve_result_from_output_border stencil.output_border target)
        //|>Seq.choose id