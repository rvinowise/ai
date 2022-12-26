
namespace rvinowise.ai


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Edges =
        open rvinowise
        open System.Collections.Generic
        open rvinowise.extensions

        let incoming_edges
            (edges: seq<#Edge>) 
            (node:Node_id) 
            =
            edges
            |>Seq.filter (fun e->e.head_id = node)

        let outgoing_edges
            (edges: #Edge seq) 
            node_id
            =
            edges
            |>Seq.filter (fun e->
                e.tail_id = node_id
            )
        
        let next_edges
            (edges: #Edge seq)
            (edge: #Edge)
            =
            edges
            |>Seq.filter (fun e->
                e.tail_id = edge.head_id
            )

        let next_vertices
            (edges: seq<Edge>) 
            (vertex: Node_id) 
            =
            vertex.id 
            |>outgoing_edges edges
            |>Seq.map (fun e->e.head)

        let previous_vertices
            (edges: seq<Edge>) 
            (vertex: Vertex) 
            =
            vertex.id
            |>incoming_edges edges
            |>Seq.map (fun e->e.tail)

        let all_vertices 
            (edges: #Edge seq)
            =
            edges
            |>Seq.collect (fun edge->[edge.tail; edge.head])
            |>Seq.distinct

        let is_first_vertex
            (edges: seq<#Edge> )
            (vertex: #Vertex)
            =
            edges
            |> Seq.exists (fun edge-> edge.head_id = vertex.id)
            |> not

        let first_vertices
            (edges: seq<#Edge>)
            =
            edges
            |>all_vertices
            |>Seq.filter (is_first_vertex edges)
            |>Seq.distinct

        let vertices_with_ids
            (edges: seq<#Edge>) 
            ids  
            =
            edges
            |>all_vertices
            |>Seq.filter (fun vertex->
                ids
                |>Set.exists (fun id -> 
                    vertex.id = id
                )
            )
        
        let rec private all_vertices_reacheble_from_vertices
            (is_needed:#Vertex->bool)
            (step_further: #Vertex -> #Vertex seq)
            (reached_goals: HashSet<#Vertex>)
            (starting_vertices: #Vertex seq)
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
        
        let all_vertices_reacheble_from_vertex//<'Vertex when 'Vertex :> Vertex>
            (is_needed:#Vertex->bool)
            (step_further: #Vertex -> #Vertex seq)
            (starting_vertex: #Vertex)
            =
            let reached_goals = HashSet<#Vertex>()
            all_vertices_reacheble_from_vertices
                is_needed
                step_further
                reached_goals
                [starting_vertex]
            reached_goals

        let vertices_reacheble_from_every_vertex
            (is_needed: #Vertex->bool)
            (step_further: #Vertex -> seq<#Vertex> )
            (starting_vertices: seq<#Vertex>)
            =
            starting_vertices
            |>Seq.map (all_vertices_reacheble_from_vertex is_needed step_further)
            |>HashSet.intersectMany

        let vertices_reacheble_from_other_vertices
            (is_needed: #Vertex->bool)
            (edges: seq<Edge>)
            (subfigures_before_goals: seq<#Vertex>)
            :HashSet<#Vertex>
            =
            vertices_reacheble_from_every_vertex
                is_needed
                (next_vertices edges)
                subfigures_before_goals

        let vertices_reaching_other_vertices
            (is_needed: #Vertex->bool)
            (edges: seq<#Edge>)
            (subfigures_after_goals: seq<#Vertex>)
            :HashSet<#Vertex>
            =
            vertices_reacheble_from_every_vertex
                is_needed
                (previous_vertices edges)
                subfigures_after_goals


        let edges_between_vertices 
            (edges:seq< #ai.Edge>)
            (vertices:Set< Vertex>)
            =
            edges
            |>Seq.filter (fun edge->
                Set.contains edge.tail vertices
                &&
                Set.contains edge.head vertices
            )

namespace rvinowise.ai.figure
    open FsUnit
    open Xunit
    
    open rvinowise.ai
    open rvinowise
    open rvinowise.extensions
    open System.Collections.Generic

    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Edges =

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

