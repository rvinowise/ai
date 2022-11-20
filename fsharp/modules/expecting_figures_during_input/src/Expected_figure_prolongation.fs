namespace rvinowise.ai

open rvinowise.ai.figure

type Expected_figure_prolongation = {
    prolongated: Figure
    expected: Subfigure Set
}


[<CompilationRepresentationAttribute(CompilationRepresentationFlags.ModuleSuffix)>]
module Expected_figure_prolongation =

    let all_subfigures_of_edges edges =
        (edges: Edge seq)
        |>Seq.collect (fun e->[e.tail; e.head])
        |>Set.ofSeq

    let first_subfigures_of_edges edges =
        edges
        |>all_subfigures_of_edges
        |>Seq.filter (
            fun s->
                edges
                |> Seq.exists (fun e-> e.head = s)
                |> not
            )
        |>Set.ofSeq

    let from_figure (figure: Figure) :Expected_figure_prolongation =
        let first_subfigures = first_subfigures_of_edges figure.edges
        {
            Expected_figure_prolongation.prolongated=figure;
            Expected_figure_prolongation.expected=first_subfigures
        }

