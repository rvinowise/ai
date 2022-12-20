namespace rvinowise.ai.figure
    open rvinowise.ai
    open System.Collections.Generic

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Edge =
        
        let next_edges 
            (edges: Edge seq)
            (edge: Edge)
            =
            edges
            |>Seq.filter (fun e->
                e.tail.id = edge.head.id
            )

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


        let rec private subfigures_reacheble_from_edges
            (reached_goals: HashSet<Node_id>)
            (all_edges)
            (reaching_edges: figure.Edge seq)
            =
            reaching_edges
            |>Seq.iter (fun edge -> 
                reached_goals.Add(edge.head.id) |> ignore
            )

            reaching_edges
            |>Seq.map (Edge.next_edges all_edges)
            |>Seq.iter (
                subfigures_reacheble_from_edges 
                    reached_goals 
                    all_edges
            )
            |>ignore

        let subfigures_reacheble_from_subfigure 
            (figure: Figure)
            (starting_subfigure:Node_id)
            =
            let edges = Subfigure.outgoing_edges figure.edges starting_subfigure
            let reached_goals = HashSet<Node_id>()
            subfigures_reacheble_from_edges 
                reached_goals
                figure.edges
                edges
            |>ignore

            reached_goals

        let subfigures_reaching_subfigure
            (figure: Figure)
            (final_subfigure:Node_id)
            =
            let edges = Subfigure.incoming_edges figure.edges final_subfigure
            let reached_goals = HashSet<Node_id>()
            subfigures_reaching_edges
                reached_goals
                figure.edges
                edges
            |>ignore

            reached_goals

        let subfigures_reacheble_from_other_subfigures
            (figure_in_which_search: Figure)
            (subfigures_before_goals: Node_id seq)
            =
            subfigures_before_goals
            |>Seq.map (
                subfigures_reacheble_from_subfigure 
                    figure_in_which_search 
            )
            |>Seq.map Set.ofSeq
            |>Seq.reduce Set.intersect
    
        let subfigures_reaching_other_subfigures
            (figure_in_which_search: Figure)
            (subfigures_after_goals: Node_id seq)
            =
            subfigures_after_goals
            |>Seq.map (
                subfigures_reaching_subfigure 
                    figure_in_which_search 
            )
            |>Seq.map Set.ofSeq
            |>Seq.reduce Set.intersect

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

        