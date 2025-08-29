namespace rvinowise.ai.example
open rvinowise
open rvinowise.ai
open rvinowise.extensions

module Stencil =

    let conditional_figure_mapped_onto_constants edges =
        edges
        |>built.Figure.simple (
             String.remove_number >>
             String.remove_hash >>
             Mapping_functions_registry.onto_exact_figure
        )|>built.Conditional_figure.from_figure_without_impossibles

    let a_fitting_stencil =
        {
            Conditional_stencil.figure =
                [
                    "b","f"
                    "h","f"
                ]
                |>conditional_figure_mapped_onto_constants
            output_border = {
                before = "b"|>Vertex_id|>Set.singleton
                after = "f"|>Vertex_id|>Set.singleton
            } 
        }

    let a_stencil_with_huge_beginning =
        {
            Conditional_stencil.figure =
                [
                    "a","f"
                    "b","f"
                    "c","f"
                    "d","f"
                    "e","f"
                    "g","f"
                    "h","f"
                    "i","f"
                    "j","f"
                    "k","f"
                    "l","f"
                    "m","f"
                    "n","f"
                    "o","f"
                    "p","f"
                    "q","f"
                ]
                |>conditional_figure_mapped_onto_constants
            output_border = {
                before = [
                        "a"
                        "b"
                        "c"
                        "d"
                        "e"
                        "g"
                        "h"
                        "i"
                        "j"
                        "k"
                        "l"
                        "m"
                        "n"
                        "o"
                        "p"
                        "q"
                    ]|>List.map Vertex_id|>Set.ofList
                after = "f1"|>Vertex_id|>Set.singleton
            } 
        }
            
    let a_long_stencil =
        {
            Conditional_stencil.figure =
                [
                    "b1","y1"
                    "b1","f1"
                    "h1","f1"
                    "y1","s1"
                    "o1","r1"
                    "r1","s1"
                    "s1","t1"
                    "f2","t1"
                ]|>conditional_figure_mapped_onto_constants
            output_border = {
                before = "f1"|>Vertex_id|>Set.singleton
                after = "o1"|>Vertex_id|>Set.singleton
            } 
        }
        