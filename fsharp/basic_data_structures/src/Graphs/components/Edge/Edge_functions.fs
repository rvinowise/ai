namespace rvinowise.ai


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Edges =
        open rvinowise
        open System.Collections.Generic
        open rvinowise.extensions

        let incoming_edges
            (edges: Edge seq) 
            (vertex: Vertex_id) 
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

        let is_last_vertex
            (edges: seq<Edge> )
            vertex
            =
            edges
            |> Seq.exists (fun edge-> edge.tail = vertex)
            |> not

        let last_vertices
            (edges: seq<Edge>)
            =
            edges
            |>all_vertices
            |>Seq.filter (is_last_vertex edges)
            |>Seq.distinct
        

        let vertex_which_goes_into_cycle
            (edges: Edge seq)
            (starting_vertex: Vertex_id)
            =

            let rec DFS_for_repetitions
                (step_further: Vertex_id -> Vertex_id seq)
                (all_visited_vertices: HashSet<Vertex_id>)
                (recursion_stack: Set<Vertex_id>)
                (current_vertex: Vertex_id)
                =

                let only_unchecked_vertices vertices =
                    vertices
                    |>Seq.filter (fun vertex->
                        all_visited_vertices.Contains(vertex)
                        |>not
                    )

                let further_vertices =
                    current_vertex
                    |>step_further

                all_visited_vertices.Add(current_vertex)|>ignore

                let repeated_vertex = 
                    further_vertices
                    |>Seq.tryFind (fun vertex->
                        Set.contains vertex recursion_stack
                    )
                match repeated_vertex with
                |Some vertex ->Some vertex
                |None->
                    further_vertices
                    |>only_unchecked_vertices
                    |>Seq.tryPick (fun next_vertex->
                        DFS_for_repetitions
                            step_further
                            all_visited_vertices
                            (recursion_stack|>Set.add current_vertex)
                            next_vertex
                    )

            DFS_for_repetitions
                (next_vertices edges)
                (HashSet<Vertex_id>())
                Set.empty
                starting_vertex


        let rec first_vertex_reacheble_from_vertices
            (is_needed:Vertex_id->bool)
            (step_further: Vertex_id -> Vertex_id seq)
            (starting_vertices: Vertex_id seq)
            =
            let further_vertices =
                starting_vertices
                |>Seq.collect step_further
            
            if Seq.length further_vertices > 0 then
                further_vertices
                |>Seq.tryFind is_needed
                |>function 
                |Some needed_vertex -> Some needed_vertex
                |None -> 
                    first_vertex_reacheble_from_vertices
                        is_needed
                        step_further
                        further_vertices
            else
                None


        let greedy_continuation
            (needed_vertices: Vertex_id Set)
            (further_vertices: Vertex_id Set)
            =
            needed_vertices
            |>Set.difference further_vertices

        let wide_continuation
            (needed_vertices: Vertex_id Set)
            (further_vertices: Vertex_id Set)
            =
            further_vertices


        let rec private all_vertices_reacheble_from_vertices
            (is_needed:Vertex_id->bool)
            (step_further: Vertex_id -> Vertex_id seq)
            (continuation: Vertex_id Set -> Vertex_id Set -> Vertex_id Set)
            (reached_goals: HashSet<Vertex_id>)
            (starting_vertices: Vertex_id seq)
            =
            let further_vertices =
                starting_vertices
                |>Seq.collect step_further
                |>Set.ofSeq
            
            if Seq.length further_vertices > 0 then
                let needed_vertices =
                    further_vertices
                    |>Set.filter is_needed
                
                needed_vertices
                |>Seq.iter (fun vertex -> 
                    reached_goals.Add(vertex) |> ignore
                )

                (needed_vertices, further_vertices) 
                ||>continuation 
                |>all_vertices_reacheble_from_vertices
                    is_needed
                    step_further
                    continuation
                    reached_goals 
            else
                ()
        
        let all_vertices_reacheble_from_vertex//<'Vertex when 'Vertex :> Vertex>
            (is_needed:Vertex_id->bool)
            (step_further: Vertex_id -> Vertex_id seq)
            (continuation: Vertex_id Set->Vertex_id Set->Vertex_id Set)
            (starting_vertex: Vertex_id)
            =
            let reached_goals = HashSet<Vertex_id>()
            all_vertices_reacheble_from_vertices
                is_needed
                step_further
                continuation
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

    
        let is_sequence (edges: Edge seq) =
            let rec only_one_next_vertex_exist
                edges 
                (vertices: Vertex_id seq) 
                =
                if Seq.length vertices = 1 then
                    vertices
                    |>Seq.head
                    |>next_vertices edges
                    |>only_one_next_vertex_exist edges 
                else Seq.isEmpty vertices
            edges
            |>first_vertices
            |>only_one_next_vertex_exist edges


        let next_vertices_of_many (edges: Edge seq) vertices =
            vertices
            |>Seq.collect (next_vertices edges)
            |>Seq.distinct



