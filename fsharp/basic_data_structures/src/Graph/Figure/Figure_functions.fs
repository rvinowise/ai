namespace rvinowise.ai

    open Xunit
    open FsUnit

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Figure=
        open rvinowise.ai.figure
        open rvinowise.extensions
        

        let subfigures_reacheble_from_other_subfigures
            (figure_in_which_search: Figure)
            (subfigures_before_goals: Node_id seq)
            =
            subfigures_before_goals
            |>Edge.subfigures_reacheble_from_other_subfigures 
                figure_in_which_search.edges
    
        let subfigures_reaching_other_subfigures
            (figure_in_which_search: Figure)
            (subfigures_after_goals: Node_id seq)
            =
            subfigures_after_goals
            |>Seq.map (
                all_subfigures_reacheble_from_subfigure
                    (Subfigure.previous_subfigures figure_in_which_search.edges)
            )
            |>HashSet.intersectMany


        [<Fact>]
        let ``subfigures reaching others``()=
            subfigures_reaching_other_subfigures
                figure.Example.a_high_level_relatively_simple_figure
                [
                    "b1";"f1"
                ]
            |> should equal ["b0"]
        [<Fact>]
        let ``subfigures reacheble from others``()=
            subfigures_reacheble_from_other_subfigures
                figure.Example.a_high_level_relatively_simple_figure
                [
                    "b0";"b2"
                ]
            |> should equal ["f1"]


        let first_subfigures (figure:Figure) =
            Edges.first_subfigures figure.edges

        let subfigure_occurances (subfigure:Figure) (figure:Figure) =
            figure.edges
            |>Seq.collect (fun e->
                [e.tail; e.head]
            )
            |>Set.ofSeq

        let lower_figures (figure:Figure) =
            figure.edges
            |>Edges.all_subfigures
            |>Subfigures.referenced_figures
            |>Set.ofSeq

        let subfigures (figure:Figure) =
            figure.edges
            |>Edges.all_subfigures

        let nodes_referencing_lower_figure figure lower_figure = 
            figure
            |> subfigures 
            |> Subfigures.pick_referencing_figure lower_figure

        let subfigures_with_ids ids (figure:Figure)=
            figure.edges
            |>  Edges.subfigures_with_ids ids


        let private edges_between_subfigures 
            (edges:Edge seq)
            (subfigures:Node_id Set)
            =
            edges
            |>Seq.filter (fun edge->
                Set.contains edge.tail.id subfigures
                &&
                Set.contains edge.head.id subfigures
            )

        let subgraph_with_subfigures 
            (target:Figure) 
            (subfigures: Node_id Set)
            =
            subfigures
            |>edges_between_subfigures target.edges
            |>Figure.stencil_output