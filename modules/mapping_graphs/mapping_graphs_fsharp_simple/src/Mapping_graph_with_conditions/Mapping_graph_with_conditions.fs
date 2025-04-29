namespace rvinowise.ai

open Xunit
open FsUnit
open rvinowise



    

    

module Mapping_graph_with_conditions = 
    
    let map_edge_onto_figure
        target
        conditions
        (tail: Figure_id)
        (head: Figure_id)
        =
        
        let does_vertex_reference_needed_figure vertex =
            Figure.reference_of_vertex target vertex =
                head 
        
        let further_step_of_searching_targets =
            Edges.next_vertices target.edges
        
        let heads_reacheble_from_tail =
            Search_in_graph.vertices_reacheble_from_vertex
                does_vertex_reference_needed_figure
                further_step_of_searching_targets
        
        Figure.all_vertices_referencing_figure tail target
        |>Seq.map (fun mapped_tail ->
            mapped_tail,
            heads_reacheble_from_tail mapped_tail
        )
    
 
    // let rec map_conditional_figure_onto_target_within_mapping
    //     target
    //     (mapping: Map<Vertex_id,Vertex_id>)
    //     (mappee: Conditional_figure)
    //     =
    //     mappee.impossible
    //     |>Seq.map(fun impossible ->
    //         map_conditional_figure_onto_target_within_mapping
    //             target
    //             mapping
    //             impossible
    //     )
    //
    // let map_figure_onto_target
    //     target
    //     (mappee: Conditional_figure)
    //     =
    //     Mapping_graph_with_immutable_mapping.map_figure_onto_target target mappee.existing
    //     |>Seq.map(fun mapping ->
    //         mappee.impossible
    //         |>Seq.map(fun impossible_figure ->
    //             map_conditional_figure_onto_target_within_mapping
    //                 target mapping impossible_figure
    //         )
    //     )
        