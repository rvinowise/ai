namespace rvinowise.ai.test

open Xunit

open rvinowise.ai
open rvinowise.ai.figure
open rvinowise.ai.ui.painted.applying_stencil

module ``application of stencils``=
    
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

    let a_fitting_stencil() =
        Figure(
            "S",
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
    let ``a fitting stencil, applied to a figure, outputs subgraphs``()=
        let target = a_high_level_relatively_simple_figure()
        let stencil = a_fitting_stencil()

        let output = Applying_stencil.results_of_stencil_application stencil target 
        ()
        //Assert.Equal(prolongation.expected, first_subfigures)

