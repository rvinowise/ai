
namespace rvinowise.ai


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Edges =
        open rvinowise
        open System.Collections.Generic
        open rvinowise.extensions

        let incoming_edges
            (edges: seq<Edge>) 
            (vertex:Vertex_id) 
            =
            edges
            |>Seq.filter (fun e->e.head = vertex)

        let outgoing_edges
            (edges: Edge seq) 
            vertex
            =
            edges
            |>Seq.filter (fun e->
                e.tail = vertex
            )
        
        let next_edges
            (edges: Edge seq)
            (edge: Edge)
            =
            edges
            |>Seq.filter (fun e->
                e.tail = edge.head
            )

        let next_vertices
            (edges: Edge seq) 
            vertex
            =
            vertex
            |>outgoing_edges edges
            |>Seq.map (fun e->e.head)

        let previous_vertices
            (edges: Edge seq) 
            vertex
            =
            vertex
            |>incoming_edges edges
            |>Seq.map (fun e->e.tail)

        let all_vertices 
            (edges: Edge seq)
            =
            edges
            |>Seq.collect (fun edge->[edge.tail; edge.head])
            |>Seq.distinct

        let is_first_vertex
            (edges: seq<Edge> )
            vertex
            =
            edges
            |> Seq.exists (fun edge-> edge.head = vertex)
            |> not

        let first_vertices
            (edges: seq<Edge>)
            =
            edges
            |>all_vertices
            |>Seq.filter (is_first_vertex edges)
            |>Seq.distinct

        
        
        let rec private all_vertices_reacheble_from_vertices
            (is_needed:Vertex_id->bool)
            (step_further: Vertex_id -> Vertex_id seq)
            (reached_goals: HashSet<Vertex_id>)
            (starting_vertices: Vertex_id seq)
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
            (is_needed:Vertex_id->bool)
            (step_further: Vertex_id -> Vertex_id seq)
            (starting_vertex: Vertex_id)
            =
            let reached_goals = HashSet<Vertex_id>()
            all_vertices_reacheble_from_vertices
                is_needed
                step_further
                reached_goals
                [starting_vertex]
            reached_goals

        let vertices_reacheble_from_every_vertex
            (is_needed: Vertex_id->bool)
            (step_further: Vertex_id -> seq<Vertex_id> )
            (starting_vertices: seq<Vertex_id>)
            =
            starting_vertices
            |>Seq.map (all_vertices_reacheble_from_vertex is_needed step_further)
            |>HashSet.intersectMany

        let vertices_reacheble_from_other_vertices
            (is_needed: Vertex_id->bool)
            (edges: Edge seq)
            (subfigures_before_goals: Vertex_id seq)
            :HashSet<Vertex_id>
            =
            vertices_reacheble_from_every_vertex
                is_needed
                (next_vertices edges)
                subfigures_before_goals

        let vertices_reaching_other_vertices
            (is_needed: Vertex_id->bool)
            (edges: Edge seq)
            (subfigures_after_goals: Vertex_id seq)
            =
            vertices_reacheble_from_every_vertex
                is_needed
                (previous_vertices edges)
                subfigures_after_goals


        let edges_between_vertices 
            (edges: Edge seq)
            (vertices:Set< Vertex_id>)
            =
            edges
            |>Seq.filter (fun edge->
                Set.contains edge.tail vertices
                &&
                Set.contains edge.head vertices
            )

    


        


