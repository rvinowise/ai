namespace rvinowise.ai.test

open Xunit

open rvinowise.ai
open rvinowise.ai.figure
open rvinowise.ai.Expecting_figures
open rvinowise.ai.Expected_figure_prolongation
open rvinowise.ai.ui.painted.Expected_figure_prolongation

module ``expectation of prolongation of figures``=
    
    let a_high_level_relatively_simple_figure() =
        Figure(
            "F",
            [
                Edge(
                    Subfigure("b"),Subfigure("c")
                );
                Edge(
                    Subfigure("b"),Subfigure("d")
                );
                Edge(
                    Subfigure("d"),Subfigure("e")
                );
                Edge(
                    Subfigure("e"),Subfigure("f")
                );
                Edge(
                    Subfigure("h"),Subfigure("f")
                );
            ]
        )

    [<Fact>]
    let ``an expected prolongation, constructed from a figure, expects its first subfigures at first``()=
        let figure_f = a_high_level_relatively_simple_figure()
        let first_subfigures = [
            Subfigure("b");
            Subfigure("h")
        ]
        let prolongation = 
            Expected_figure_prolongation.from_figure figure_f
        Assert.Equal(prolongation.expected, first_subfigures)


    [<Fact>]
    let ``prolongating a figure with a new input changes expectations``()=
        let high_figure = a_high_level_relatively_simple_figure()
        let expected_subfigures_after_b = [
            Subfigure("c");
            Subfigure("d");
            Subfigure("h");
        ]
        let expected_subfigures_after_d = [
            Subfigure("c");
            Subfigure("e");
            Subfigure("h");
        ]
        let initial_expectation = from_figure high_figure
        let next_expectation = 
            prolongate_a_figure_with_an_input_figure "b" initial_expectation 
        Assert.Equal(
            next_expectation.expected,
            expected_subfigures_after_b
        )
        let next_expectation = 
            prolongate_a_figure_with_an_input_figure "d" next_expectation 
        Assert.Equal(
            next_expectation.expected,
            expected_subfigures_after_d
        )

    [<Fact>]
    let paint_expectation()=
        let high_figure = a_high_level_relatively_simple_figure()
        let initial_expectation = from_figure high_figure
        
        high_figure.id
        |>empty_root_graph 
        |>provide_clastered_subgraph_inside_root_graph "step1" initial_expectation
        |>provide_clastered_subgraph_inside_root_graph "step2" initial_expectation
        |>provide_clastered_subgraph_inside_root_graph "step3" initial_expectation
        |>open_image_of_graph