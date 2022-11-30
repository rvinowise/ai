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
module Edge =
    
    let all_subfigures edges =
        (edges: Edge seq)
        |>Seq.collect (fun e->[e.tail; e.head])
        |>Set.ofSeq
    
    let first_subfigures edges =
        edges
        |>all_subfigures
        |>Seq.filter (
            fun s->
                edges
                |> Seq.exists (fun e-> e.head = s)
                |> not
            )
        |>Set.ofSeq

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
module Edge =
    
    let all_nodes edges =
        (edges: Edge seq)
        |>Seq.collect (fun e->[e.tail; e.head])
        |>Set.ofSeq
    
    let first_nodes edges =
        edges
        |>all_nodes
        |>Seq.filter (
            fun s->
                edges
                |> Seq.exists (fun e-> e.head = s)
                |> not
            )
        |>Set.ofSeq

    
