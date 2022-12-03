namespace rvinowise.ai.test

open Xunit

open rvinowise
open rvinowise.ai
open rvinowise.ai.figure
open rvinowise.ai.ui.painted.applying_stencil
open rvinowise.ai.ui

module ``application of stencils``=

    open System.Text.RegularExpressions

    let remove_number label =
            Regex.Replace(label, @"[^a-zA-Z]", "");

    let Subfigure id =
        
        ai.figure.Subfigure(
            id,
            remove_number id
        )

    open rvinowise.ai.stencil

    type Used_figures()=
        member _.a_fitting_stencil: Stencil =
            rvinowise.ai.Stencil(
                "S",
                [
                    stencil.Edge(
                        Node("b"), Node.stencil_out("out1")
                    );
                    stencil.Edge(
                        Node.stencil_out("out1"), Node("f")
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
                    figure.Edge(
                        Subfigure("b0"),Subfigure("c")
                    );
                    figure.Edge(
                        Subfigure("b0"),Subfigure("d")
                    );
                    figure.Edge(
                        Subfigure("c"),Subfigure("b1")
                    );
                    figure.Edge(
                        Subfigure("d"),Subfigure("e")
                    );
                    figure.Edge(
                        Subfigure("d"),Subfigure("f0")
                    );
                    figure.Edge(
                        Subfigure("e"),Subfigure("f1")
                    );
                    figure.Edge(
                        Subfigure("h"),Subfigure("f1")
                    );
                    figure.Edge(
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
                "target figure" 
                (painted.Figure.painted_edges figure.edges)
            |>painted.Figure.provide_clustered_subgraph_inside_root_graph 
                "stencil"
                (painted.Stencil.painted_edges stencil.edges)
            |>painted.Figure.open_image_of_graph

        