namespace rvinowise.ai

module Mapping_graph_tightly = 
    open rvinowise.ai.generating_combinations
    open rvinowise.ai.stencil
    open rvinowise
    open rvinowise.ai.mapping_graph_impl

    open Xunit
    open FsUnit


    let map_figure_onto_target
        target
        mappee
        =
        target
        |>map_first_nodes mappee
        |>prolongate_all_mappings
            mappee 
            target
            (Figure.first_vertices mappee)