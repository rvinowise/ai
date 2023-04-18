namespace rvinowise.ai

module Applying_stencil = 
    open System.Collections.Generic
    open rvinowise.ai.generating_combinations
    open rvinowise.ai.stencil
    open rvinowise
    open rvinowise.ai.mapping_graph_impl
 

    let retrieve_result 
        stencil
        (target:Figure)
        mapping =
            let output_node = 
                stencil
                |>Stencil.output

            let output_beginning =
                output_node
                |>Edges.previous_vertices stencil.edges
                |>Mapping.targets_of_mapping mapping
                |>Edges.vertices_reacheble_from_other_vertices
                    (fun _->true)
                    target.edges
                |>Set.ofSeq

            let output_ending =
                output_node
                |>Edges.next_vertices stencil.edges
                |>Mapping.targets_of_mapping mapping //bug
                |>Edges.vertices_reaching_other_vertices
                    (fun _->true)
                    target.edges
                |>Set.ofSeq
            
            output_beginning
            |>Set.intersect output_ending
            |>function
            |set when set|>Set.count>0 -> Some set
            |_->None
            |>Option.map ( built.Figure.subgraph_with_vertices target)
            //|>built.Figure.is_empty


    let results_of_stencil_application
        target
        stencil
        =
        stencil
        |>Figure_from_stencil.convert
        |>Mapping_graph.map_figure_onto_target target
        |>Seq.map (retrieve_result stencil target)
        |>Seq.choose id

    
