namespace rvinowise.ai.figure
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

namespace rvinowise.ai

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Stencil =
        
        let first_subfigures (stencil:Stencil) =
            stencil.edges
            |>rvinowise.ai.stencil.Edges.first_nodes 
            |>Nodes.only_subfigures

        