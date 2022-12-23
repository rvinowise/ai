namespace rvinowise.ai
    open rvinowise.ai

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Figure=

        

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