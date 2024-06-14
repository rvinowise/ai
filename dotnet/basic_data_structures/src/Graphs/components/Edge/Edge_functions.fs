namespace rvinowise.ai

open System.Collections.Generic

module Edges =

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
        |>Set.map (_.head)

    let previous_vertices
        (edges: Edge Set) 
        vertex
        =
        vertex
        |>incoming_edges edges
        |>Set.map (_.tail)

    let all_vertices 
        (edges: Edge Set)
        =
        edges
        |>Seq.collect (fun edge->[edge.tail; edge.head])
        |>Set.ofSeq

    let is_first_vertex
        (edges: Edge Set)
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
        (edges: Edge Set)
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



    let edges_between_vertices 
        (edges: Edge seq)
        (vertices: Vertex_id Set)
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



