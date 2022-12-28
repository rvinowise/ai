namespace rvinowise.ai.test

open Xunit
open FsUnit

open rvinowise.ai
open rvinowise.ai.figure
open rvinowise.ai.Expecting_figures
open rvinowise.ai.Expected_figure_prolongation
open rvinowise.ai.ui.painted.Expected_figure_prolongation
open rvinowise.ai.ui.painted

module ``expectation of prolongation of figures``=
    
    let a_high_level_relatively_simple_figure =
        rvinowise.ai.figure.built.simple
            "F"
            [
                "b","c";
                "b","d";
                "d","e";
                "e","f";
                "h","f"
            ]
        

    [<Fact>]
    let ``an expected prolongation, constructed from a figure, expects its first subfigures at first``()=
        let figure_f = a_high_level_relatively_simple_figure
        let first_subfigures = [
            "b";"h"
        ]
        let prolongation = 
            Expected_figure_prolongation.from_figure figure_f
        
        prolongation.expected |> should equal first_subfigures


    [<Fact>]
    let ``prolongating a figure with a new input changes expectations``()=
        let high_figure = a_high_level_relatively_simple_figure
        let expected_subfigures_after_b = [
            "c";"d";"h";
        ]
        let expected_subfigures_after_d = [
            "c";"e";"h";
        ]
        let initial_expectation = from_figure high_figure
        let next_expectation = 
            prolongate_expectation_with_an_input_figure "b" initial_expectation 
        
        next_expectation.expected
        |> should equal expected_subfigures_after_b
        
        let next_expectation = 
            prolongate_expectation_with_an_input_figure "d" next_expectation 
        
        next_expectation.expected
        |> should equal expected_subfigures_after_d

    [<Fact>] //(Skip="ui")
    let paint_expectation()=
        let high_figure = a_high_level_relatively_simple_figure
        let initial_expectation = from_figure high_figure
        let expectation_after_b = 
            prolongate_expectation_with_an_input_figure "b" initial_expectation
        let expectation_after_d = 
            prolongate_expectation_with_an_input_figure "d" expectation_after_b
        
        high_figure.graph.id
        |>Graph.empty_root_graph 
        |>provide_expected_prolongation_inside_graph "initial_expectation" initial_expectation
        |>provide_expected_prolongation_inside_graph "expectation_after_b" expectation_after_b
        |>provide_expected_prolongation_inside_graph "expectation_after_d" expectation_after_d
        |>Graph.open_image_of_graph