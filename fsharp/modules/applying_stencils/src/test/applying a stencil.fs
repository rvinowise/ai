namespace rvinowise.ai.test

open Xunit

open rvinowise.ai
open rvinowise.ai.figure
open rvinowise.ai.ui.painted.applying_stencil
open rvinowise.ai.ui

module ``application of stencils``=
    
    type Used_figures()=
        member _.a_high_level_relatively_simple_figure =
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

        member _.a_fitting_stencil =
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

    type tests(used_figures: Used_figures)=
        interface IClassFixture<Used_figures>

        member _.figures = used_figures

        [<Fact>]
        member this.``a fitting stencil, applied to a figure, outputs subgraphs``()=
            let target = this.figures.a_high_level_relatively_simple_figure
            let stencil = this.figures.a_fitting_stencil

            let output = Applying_stencil.results_of_stencil_application stencil target 
            ()
            //Assert.Equal(prolongation.expected, first_subfigures)

        [<Fact>]
        member this.``preparing inputs for permutators, which map initial nodes``()=
            let permutator_input = Applying_stencil.input_for_first_mappings_permutators 
                                    this.figures.a_fitting_stencil
                                    this.figures.a_high_level_relatively_simple_figure
            ()

        [<Fact>]
        member this.``paint target figure``()=
            let figure: Figure = this.figures.a_high_level_relatively_simple_figure

            figure.id
            |>painted.Graph.empty_root_graph 
            |>painted.Figure.provide_clustered_subgraph_inside_root_graph 
                "target figure" figure.edges
            |>painted.Figure.open_image_of_graph

        