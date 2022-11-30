namespace rvinowise.ai.test

open Xunit

open rvinowise
open rvinowise.ai
open rvinowise.ai.figure
open rvinowise.ai.ui.painted.applying_stencil
open rvinowise.ai.ui

module ``application of stencils``=
    
    let Subfigure id =
        ai.Subfigure(
            id,
            Node.label_from_id id
        )

    open rvinowise.ai.Node

    type Used_figures()=
        member _.a_fitting_stencil: Stencil =
            rvinowise.ai.Stencil(
                "S",
                [
                    stencil.Edge(
                        Node("b"), stencil_out("out1")
                    );
                    stencil.Edge(
                        stencil_out("out1"), Node("f")
                    );
                    stencil.Edge(
                        Node("h"),Node("f")
                    );
                   
                ]
            )

        member _.a_high_level_relatively_simple_figure =
            Figure(
                "F",
                [
                    Edge(
                        Subfigure("b0"),Subfigure("c")
                    );
                    Edge(
                        Subfigure("b0"),Subfigure("d")
                    );
                    Edge(
                        Subfigure("c"),Subfigure("b1")
                    );
                    Edge(
                        Subfigure("d"),Subfigure("e")
                    );
                    Edge(
                        Subfigure("d"),Subfigure("f0")
                    );
                    Edge(
                        Subfigure("e"),Subfigure("f1")
                    );
                    Edge(
                        Subfigure("h"),Subfigure("f1")
                    );
                    Edge(
                        Subfigure("b2"),Subfigure("h")
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

        [<Fact>] //(Skip="ui")
        member this.``paint the target figure and the stencil``()=
            let figure = this.figures.a_high_level_relatively_simple_figure
            let stencil: rvinowise.ai.Stencil = this.figures.a_fitting_stencil

            figure.id
            |>painted.Graph.empty_root_graph 
            |>painted.Figure.provide_clustered_subgraph_inside_root_graph 
                "target figure" figure.edges
            |>painted.Figure.provide_clustered_subgraph_inside_root_graph 
                "stencil" stencil.edges
            |>painted.Figure.open_image_of_graph

        