namespace rvinowise.ai.figure
    open rvinowise.ai

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Edges =
        
        let all_subfigures edges =
            (edges: Edge seq)
            |>Seq.collect (fun e->[e.tail; e.head])
            |>Seq.distinct
        
        let first_subfigures edges =
            edges
            |>all_subfigures
            |>Seq.filter (
                fun s->
                    edges
                    |> Seq.exists (fun e-> e.head = s)
                    |> not
                )
            |>Seq.distinct

        

        let subfigures_with_ids ids edges  =
            edges
            |>all_subfigures
            |>Seq.filter (fun subfigure->
                ids
                |>Set.exists (fun id -> 
                    subfigure.id = id
                )
            )

        let starting_from (subfigure: Node_id) (edges: Edge seq) =
            edges
            |>Seq.filter (fun e->e.tail.id = subfigure)
            |>Seq.map (fun e->e.head)

namespace rvinowise.ai.stencil
    open rvinowise.ai

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Edges =
        
        let all_nodes edges =
            (edges: Edge seq)
            |>Seq.collect (fun e->[e.tail; e.head])
            |>Seq.distinct
        
        let first_nodes edges =
            edges
            |>all_nodes
            |>Seq.filter (
                fun s->
                    edges
                    |> Seq.exists (fun e-> e.head = s)
                    |> not
                )
            |>Seq.distinct

        let starting_from (node: Node_id) (edges: Edge seq) =
            edges
            |>Seq.filter (fun e->e.tail.id = node)
            |>Seq.map (fun e->e.head)