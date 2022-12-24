
namespace rvinowise.ai
    open rvinowise


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Edges =
        
        let incoming_edges<'Edge when 'Edge :> ai.Edge> 
            (edges: 'Edge seq) 
            (node:Node_id) 
            =
            edges
            |>Seq.filter (fun e->e.head_id = node)

        let outgoing_edges<'Edge when 'Edge :> ai.Edge> 
            (edges: 'Edge seq) 
            node_id
            =
            edges
            |>Seq.filter (fun e->
                e.tail_id = node_id
            )
        
        let next_edges<'Edge when 'Edge :> ai.Edge>  
            (edges: 'Edge seq)
            (edge: 'Edge)
            =
            edges
            |>Seq.filter (fun e->
                e.tail_id = edge.head_id
            )

        let all_nodes<'Edge when 'Edge :> ai.Edge> 
            (edges: 'Edge seq)
            =
            edges
            |>Seq.collect (fun edge->[edge.tail_id; edge.head_id])
            |>Seq.distinct

        let first_nodes<'Edge when 'Edge :> ai.Edge> 
            (edges: 'Edge seq)
            =
            edges
            |>all_nodes
            |>Seq.filter (
                fun node->
                    edges
                    |> Seq.exists (fun edge-> edge.head_id = node)
                    |> not
                )
            |>Seq.distinct

        


namespace rvinowise.ai.figure
    open FsUnit
    open Xunit
    open System.Collections.Generic
    open rvinowise.ai
    open rvinowise
    open rvinowise.extensions

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Edges =
        
        let next_subfigures 
            (edges: figure.Edge seq)
            (subfigure: Node_id)
            =
            edges
            |>Seq.filter (fun e->e.tail.id = subfigure)
            |>Seq.map (fun e->e.head)

        let previous_subfigures 
            (edges: figure.Edge seq)
            (subfigure: Node_id)
            =
            edges
            |>Seq.filter (fun e->e.head.id = subfigure)
            |>Seq.map (fun e->e.tail)

        let all_subfigures edges =
            (edges: figure.Edge seq)
            |>Seq.collect (fun e->[e.tail; e.head])
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

        let subfigures_starting_from (subfigure: Node_id) (edges: figure.Edge seq) =
            edges
            |>Seq.filter (fun e->e.tail.id = subfigure)
            |>Seq.map (fun e->e.head)


        let rec private all_subfigures_reacheble_from_subfigures
            (reached_goals: HashSet<Node_id>)
            (step_further: Node_id -> Subfigure seq)
            (starting_subfigures: Node_id seq)
            =
            let further_subfigures =
                starting_subfigures
                |>Seq.collect step_further
                |>Subfigures.ids
            
            if Seq.length further_subfigures > 0 then
                further_subfigures
                |>Seq.iter (fun subfigure -> 
                    reached_goals.Add(subfigure) |> ignore
                )

                further_subfigures
                |>all_subfigures_reacheble_from_subfigures 
                    reached_goals 
                    step_further
            else
                ()
        
        let private all_subfigures_reacheble_from_subfigure
            (step_further: Node_id -> Subfigure seq)
            (starting_subfigure: Node_id)
            =
            let reached_goals = HashSet<Node_id>()
            all_subfigures_reacheble_from_subfigures
                reached_goals
                step_further
                [starting_subfigure]
            reached_goals

        let private subfigures_reacheble_from_every_subfigure
            (step_further: Node_id -> Subfigure seq)
            (starting_subfigures: Node_id seq)
            =
            starting_subfigures
            |>Seq.map (all_subfigures_reacheble_from_subfigure step_further)
            |>HashSet.intersectMany


        let subfigures_reacheble_from_other_subfigures
            (edges: figure.Edge seq)
            (subfigures_before_goals: Node_id seq)
            =
            subfigures_before_goals
            |>subfigures_reacheble_from_every_subfigure
                (next_subfigures edges)
    
        let subfigures_reaching_other_subfigures
            (edges: figure.Edge seq)
            (subfigures_after_goals: Node_id seq)
            =
            subfigures_after_goals
            |>Seq.map (
                all_subfigures_reacheble_from_subfigure
                    (previous_subfigures edges)
            )
            |>HashSet.intersectMany



    

namespace rvinowise.ai.stencil
    open rvinowise.ai

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Edges =
        
        

        let all_nodes edges =
            (edges: stencil.Edge seq)
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
            (edges: stencil.Edge seq) (node:Node_id) 
            =
            node 
            |>Edges.outgoing_edges edges
            |>Seq.map (fun e->e.head)

        let previous_nodes
            (edges: stencil.Edge seq) node
            =
            node
            |>Edges.incoming_edges edges
            |>Seq.map (fun e->e.tail)

        let next_subfigures 
            (edges: stencil.Edge seq)
            (node: Node_id)
            =
            node
            |>next_nodes edges
            |>Nodes.only_subfigures    

        let previous_subfigures
            (edges: stencil.Edge seq) node 
            =
            node
            |>previous_nodes edges
            |>Nodes.only_subfigures

        let previous_subfigures_jumping_over_outputs
            (edges: stencil.Edge seq) node 
            =
            node
            |>Edges.incoming_edges edges
            |>Seq.collect (fun edge->
                match Subfigure.ofNode edge.tail with
                |Some previous_subfigure -> Seq.ofList [previous_subfigure]
                |None -> (previous_subfigures edges edge.tail.id)
            )

        let nodes_starting_from 
            (node: Node_id) 
            (edges: stencil.Edge seq) 
            =
            edges
            |>Seq.filter (fun e->e.tail.id = node)
            |>Seq.map (fun e->e.head)

