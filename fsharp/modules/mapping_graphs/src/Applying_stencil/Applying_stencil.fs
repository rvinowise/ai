namespace rvinowise.ai

module Applying_stencil = 
    open System.Collections.Generic
    open rvinowise.ai.generating_combinations
    open rvinowise.ai.stencil
    open rvinowise
    open rvinowise.ai.mapping_graph_impl
 

    let is_figure_without_impossible_parts 
        (impossibles: Figure seq)
        (owner_figure: Figure)
        =
        impossibles
        |>Seq.collect (Mapping_graph.map_figure_onto_target owner_figure)
        |>Seq.isEmpty

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
                    Edges.continue_search_till_end
                    (fun _->true)
                    target.edges
                |>Set.ofSeq

            let output_ending =
                output_node
                |>Edges.next_vertices stencil.edges
                |>Mapping.targets_of_mapping mapping
                |>Edges.vertices_reaching_other_vertices
                    Edges.continue_search_till_end
                    (fun _->true)
                    target.edges
                |>Set.ofSeq
            
            (output_beginning,output_ending)
            ||>Set.intersect 
            |>Some
            |>Option.filter (Set.isEmpty>>not)
            |>Option.map (built.Figure.subgraph_with_vertices target)
            |>Option.filter (is_figure_without_impossible_parts stencil.output_without)
            


    let results_of_stencil_application
        target
        stencil
        =
        stencil
        |>Figure_from_stencil.convert
        |>Mapping_graph.map_figure_onto_target target
        |>Seq.map (retrieve_result stencil target)
        |>Seq.choose id

    
