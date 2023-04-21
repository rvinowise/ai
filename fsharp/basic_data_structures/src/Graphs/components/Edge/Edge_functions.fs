namespace rvinowise.ai


    [<CompilationRepresentation(CompilationRepresentationFlags.ModuleSuffix)>]
    module Edges =
        open rvinowise
        open System.Collections.Generic
        open rvinowise.extensions

        let incoming_edges
            (edges: Edge Set) 
            (vertex: Vertex_id) 
            =
            edges
            |>Set.filter (fun e->e.head = vertex)

        let outgoing_edges
            (edges: Edge Set) 
            vertex
            =
            edges
            |>Set.filter (fun e->
                e.tail = vertex
            )
        
        let next_edges
            (edges: Edge Set)
            (edge: Edge)
            =
            edges
            |>Set.filter (fun e->
                e.tail = edge.head
            )

        let next_vertices
            (edges: Edge Set) 
            vertex
            =
            vertex
            |>outgoing_edges edges
            |>Set.map (fun e->e.head)

        let previous_vertices
            (edges: Edge Set) 
            vertex
            =
            vertex
            |>incoming_edges edges
            |>Set.map (fun e->e.tail)

        let all_vertices 
            (edges: Edge Set)
            =
            edges
            |>Seq.collect (fun edge->[edge.tail; edge.head])
            |>Set.ofSeq

        let is_first_vertex
            (edges: Edge Set )
            vertex
            =
            edges
            |> Set.exists (fun edge-> edge.head = vertex)
            |> not

        let first_vertices
            (edges: Edge Set)
            =
            edges
            |>all_vertices
            |>Set.filter (is_first_vertex edges)

        let is_last_vertex
            (edges: Edge Set )
            vertex
            =
            edges
            |> Set.exists (fun edge-> edge.tail = vertex)
            |> not

        let last_vertices
            (edges: Edge Set)
            =
            edges
            |>all_vertices
            |>Set.filter (is_last_vertex edges)
        

        let vertex_which_goes_into_cycle
            (edges: Edge Set)
            (starting_vertex: Vertex_id)
            =

            let rec DFS_for_repetitions
                (step_further: Vertex_id -> Vertex_id Set)
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



        let only_first_suitable_vertices
            (needed_vertices: Vertex_id Set)
            (further_vertices: Vertex_id Set)
            =
            needed_vertices
            |>Set.difference further_vertices

        let continue_search_till_end
            (needed_vertices: Vertex_id Set)
            (further_vertices: Vertex_id Set)
            =
            further_vertices


        let rec private vertices_reacheble_from_vertices
            (is_vertex_needed:Vertex_id->bool)
            (step_further: Vertex_id -> Vertex_id Set)
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
                    |>Set.filter is_vertex_needed
                
                needed_vertices
                |>Seq.iter (fun vertex -> 
                    reached_goals.Add(vertex) |> ignore
                )

                (needed_vertices, further_vertices) 
                ||>continue_search_till_end 
                |>vertices_reacheble_from_vertices
                    is_vertex_needed
                    step_further
                    reached_goals 
            else
                ()
        
        let vertices_reacheble_from_vertex//<'Vertex when 'Vertex :> Vertex>
            (is_vertex_needed:Vertex_id->bool)
            (step_further: Vertex_id -> Vertex_id Set)
            (starting_vertex: Vertex_id)
            =
            let reached_goals = HashSet<Vertex_id>()
            vertices_reacheble_from_vertices
                is_vertex_needed
                step_further
                reached_goals
                [starting_vertex]
            reached_goals

        let ``first_encountered_vertex(simple)`` 
            (step_further: Vertex_id -> Vertex_id Set)
            (vertices: HashSet<Vertex_id>)
            =
            //let vertices = set vertices
            let reached_vertices =
                vertices
                |>Seq.fold(fun (set:HashSet<Vertex_id>) vertex->
                        vertex
                        |>vertices_reacheble_from_vertex
                            (fun vertex->vertices.Contains(vertex))
                            step_further
                        |>set.UnionWith
                        |>ignore
                        set
                    )
                    (HashSet<Vertex_id>())
            let unreached_vertices = 
                vertices.ExceptWith reached_vertices
            unreached_vertices

        let vertices_reacheble_from_every_vertex
            (is_vertex_needed: Vertex_id->bool)
            (step_further: Vertex_id -> Vertex_id Set)
            (starting_vertices: Vertex_id seq)
            =
            starting_vertices
            |>Seq.map (
                vertices_reacheble_from_vertex 
                    is_vertex_needed
                    step_further
            )
            |>HashSet.intersectMany
            |>``first_encountered_vertex(simple)``

        let search_vertices_forward
            continuation
            (is_vertex_needed: Vertex_id->bool)
            (edges: Edge Set)
            (starting_vertices: Vertex_id Set)
            =
            vertices_reacheble_from_every_vertex
                continuation
                is_vertex_needed
                (next_vertices edges)
                starting_vertices

        let search_vertices_backward
            continuation
            (is_vertex_needed: Vertex_id->bool)
            (edges: Edge Set)
            (starting_vertices: Vertex_id Set)
            =
            vertices_reacheble_from_every_vertex
                continuation
                is_vertex_needed
                (previous_vertices edges)
                starting_vertices


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

    
        let is_sequence (edges: Edge Set) =
            let rec only_one_next_vertex_exist
                edges 
                (vertices: Vertex_id Set) 
                =
                if Set.count vertices = 1 then
                    vertices
                    |>Seq.head
                    |>next_vertices edges
                    |>only_one_next_vertex_exist edges 
                else Set.isEmpty vertices
            edges
            |>first_vertices
            |>only_one_next_vertex_exist edges


        let next_vertices_of_many (edges: Edge Set) vertices =
            vertices
            |>Seq.collect (next_vertices edges)
            |>Set.ofSeq



