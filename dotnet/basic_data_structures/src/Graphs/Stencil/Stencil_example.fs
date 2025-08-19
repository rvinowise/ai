namespace rvinowise.ai.example
open rvinowise.ai

module Stencil =


    let a_fitting_stencil =
        {
            Conditional_stencil.figure =
                [
                    "b1","f1"
                    "h1","f1"
                ]|>built.Figure.simple_without_separator
                |>built.Conditional_figure.from_figure_without_impossibles
            output_border = {
                before = "b1"|>Vertex_id|>Set.singleton
                after = "f1"|>Vertex_id|>Set.singleton
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
                ]|>built.Figure.simple_without_separator
                |>built.Conditional_figure.from_figure_without_impossibles
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
                ]|>built.Figure.simple_without_separator
                |>built.Conditional_figure.from_figure_without_impossibles
            output_border = {
                before = "f1"|>Vertex_id|>Set.singleton
                after = "o1"|>Vertex_id|>Set.singleton
            } 
        }
        