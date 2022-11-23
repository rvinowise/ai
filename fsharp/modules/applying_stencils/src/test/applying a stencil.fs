namespace rvinowise.ai.test

open Xunit

open rvinowise.ai
open rvinowise.ai.figure
open rvinowise.ai.ui.painted.applying_stencil
open rvinowise.ai.ui

module ``application of stencils``=
    
    let a_high_level_relatively_simple_figure =
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

    let a_fitting_stencil =
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
                    Subfigure("c"),Subfigure("b")
                );
                Edge(
                    Subfigure("d"),Subfigure("e")
                );
                Edge(
                    Subfigure("d"),Subfigure("f")
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
        let target = a_high_level_relatively_simple_figure
        let stencil = a_fitting_stencil

        let output = Applying_stencil.results_of_stencil_application stencil target 
        ()
        //Assert.Equal(prolongation.expected, first_subfigures)

    [<Fact>]
    let ``preparing inputs for permutators, which map initial nodes``()=
        let permutator_input = Applying_stencil.input_for_first_mappings_permutators 
                                a_fitting_stencil
                                a_high_level_relatively_simple_figure
        ()

    [<Fact(Skip="ui")>]
    let ``paint target figure``()=
        let figure: Figure = a_high_level_relatively_simple_figure

        figure.id
        |>painted.Graph.empty_root_graph 
        |>painted.Figure.provide_clustered_subgraph_inside_root_graph 
            "target figure" figure
        |>painted.Figure.open_image_of_graph
