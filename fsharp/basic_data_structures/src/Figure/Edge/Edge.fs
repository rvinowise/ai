namespace rvinowise.ai.figure
    open rvinowise.ai

    (* figure.Edge only connect subfigures, but stencil.Edge can either connect subfigures or stencil outputs  *)

    type Edge = 
        struct
            val tail: Subfigure
            val head: Subfigure

            new (tail, head) =
                {tail = tail; head = head;}
        end

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

namespace rvinowise.ai.stencil
    open rvinowise.ai

    type Edge = 
        struct
            val tail: Node
            val head: Node

            new (tail, head) =
                {tail = tail; head = head;}
        end

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

        let next_nodes 
            (edges: Edge seq)
            (node: Node_id) 
            =
            edges
            |>Seq.filter (fun e->e.tail.id = node)
            |>Seq.map (fun e->e.head)
