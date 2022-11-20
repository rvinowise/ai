namespace rvinowise.ai.test

open rvinowise.ai
open rvinowise.ai.figure
open Expected_figure_prolongation

open Xunit

module ``expectation of prolongation of figures``=

    [<Fact>]
    let ``expected prolongation constructed from a figure expects its first subfigures at first``()=
        let figure_f = Figure(
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
        let first_subfigures = [
            Subfigure("b");
            Subfigure("h")
        ]
        let prolongation = 
            from_figure figure_f
        Assert.Equal(prolongation.expected, first_subfigures)


    [<Fact>]
    let ``prolongating a figure with a new input changes expectations``()=
        let high_figure = Figure(
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
        let expected_subfigures_after_b = [
            Subfigure("c");
            Subfigure("d");
            Subfigure("f");
        ]
        let expected_subfigures_after_d = [
            Subfigure("c");
            Subfigure("e");
            Subfigure("f");
        ]
        let initial_expectation = Expected_figure_prolongation.from_figure high_figure
        let next_expectation = prolongate_a_figure_with_an_input_figure initial_expectation Figure("b")
        Assert.Equal(
            next_expectation.expected,
            expected_subfigures_after_b
        )
        let next_expectation = prolongate_a_figure_with_an_input_figure next_expectation Figure("d")
        Assert.Equal(
            next_expectation.expected,
            expected_subfigures_after_d
        )