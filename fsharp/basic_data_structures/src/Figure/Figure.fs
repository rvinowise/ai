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


    let first_nodes (figure:Figure) =
        Edge.first_nodes figure.edges

    let subfigure_occurances (subfigure:Figure) (figure:Figure) =
        figure.edges
        |>Seq.collect (fun e->
            [e.tail; e.head]
        )
        |>Set.ofSeq

    let lower_figures (figure:Figure) =
        figure.edges
        |>Edge.all_nodes
        |>Subfigure.participating_figures
        |>Set.ofSeq

    let subfigures (figure:Figure) =
        figure.edges
        |>Edge.all_nodes

    let nodes_referencing_lower_figure figure lower_figure = 
        figure
        |> subfigures 
        |> Seq.filter (fun s->
            match s.referenced with
            |Subfigure_reference.Lower_figure id ->
                id = lower_figure
            | _ -> false
        )