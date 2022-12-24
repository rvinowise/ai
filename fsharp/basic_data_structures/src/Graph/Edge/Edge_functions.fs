
namespace rvinowise.ai


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Edges =
        open rvinowise
        open System.Collections.Generic
        open rvinowise.extensions

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

        let next_vertices<'Edge,'Vertex 
            when 'Edge :> ai.Edge and 'Vertex :> ai.Vertex>  
            (edges: 'Edge seq) 
            (vertex: 'Vertex) 
            =
            vertex.id 
            |>outgoing_edges edges
            |>Seq.map (fun e->e.head)

        let previous_vertices<'Edge,'Vertex 
            when 'Edge :> ai.Edge and 'Vertex :> ai.Vertex>  
            (edges: 'Edge seq) 
            (vertex: 'Vertex) 
            =
            vertex.id
            |>incoming_edges edges
            |>Seq.map (fun e->e.tail)

        let all_vertices<'Edge when 'Edge :> ai.Edge> 
            (edges: 'Edge seq)
            =
            edges
            |>Seq.collect (fun edge->[edge.tail; edge.head])
            |>Seq.distinct

        let is_first_vertex<'Edge when 'Edge :> ai.Edge> 
            (edges: 'Edge seq)
            (vertex:Vertex)
            =
            edges
            |> Seq.exists (fun edge-> edge.head_id = vertex.id)
            |> not

        let first_vertices<'Edge when 'Edge :> ai.Edge> 
            (edges: 'Edge seq)
            =
            edges
            |>all_vertices
            |>Seq.filter (is_first_vertex edges)
            |>Seq.distinct

        
        let rec private all_vertices_reacheble_from_vertices<'Vertex when 'Vertex :> Vertex>
            (is_needed:'Vertex->bool)
            (step_further: 'Vertex -> 'Vertex seq)
            (reached_goals: HashSet<'Vertex>)
            (starting_vertices: 'Vertex seq)
            =
            let further_vertices =
                starting_vertices
                |>Seq.collect step_further
            
            if Seq.length further_vertices > 0 then
                further_vertices
                |>Seq.filter is_needed
                |>Seq.iter (fun vertex -> 
                    reached_goals.Add(vertex) |> ignore
                )

                further_vertices
                |>all_vertices_reacheble_from_vertices
                    is_needed
                    step_further
                    reached_goals 
            else
                ()
        
        let all_vertices_reacheble_from_vertex<'Vertex when 'Vertex :> Vertex>
            (is_needed:'Vertex->bool)
            (step_further: 'Vertex -> 'Vertex seq)
            (starting_vertex: 'Vertex)
            =
            let reached_goals = HashSet<'Vertex>()
            all_vertices_reacheble_from_vertices
                is_needed
                step_further
                reached_goals
                [starting_vertex]
            reached_goals

        let vertices_reacheble_from_every_vertex<'Vertex when 'Vertex :> Vertex>
            is_needed
            step_further
            starting_vertices
            =
            starting_vertices
            |>Seq.map (all_vertices_reacheble_from_vertex is_needed step_further)
            |>HashSet.intersectMany


namespace rvinowise.ai.figure
    open FsUnit
    open Xunit
    
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

        let first_subfigures
            (edges: figure.Edge seq)
            =
            edges
            |>all_subfigures
            |>Seq.filter (Edges.is_first_vertex edges)
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


        

        let subfigures_reacheble_from_other_subfigures
            (is_needed:Subfigure->bool)
            (edges: figure.Edge seq)
            (subfigures_before_goals: Node_id seq)
            =
            subfigures_before_goals
            |>Edges.vertices_reacheble_from_every_vertex
                is_needed
                (next_subfigures edges)
    
        let subfigures_reaching_other_subfigures
            (is_needed:Subfigure->bool)
            (edges: figure.Edge seq)
            (subfigures_after_goals: Node_id seq)
            =
            subfigures_after_goals
            |>Seq.map (
                Edges.all_vertices_reacheble_from_vertex
                    is_needed
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

