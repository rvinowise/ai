namespace rvinowise.ai.figure

open System.Collections.Generic
open rvinowise.ai

type Figure(
    id, 
    edges
) =
    member this.id:Figure_id = id
    member this.edges: Edge seq = edges
    
    new (id) =
        Figure(id,[])



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