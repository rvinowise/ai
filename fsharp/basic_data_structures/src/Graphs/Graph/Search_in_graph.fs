module rvinowise.ai.Search_in_graph
    open rvinowise
    open System.Collections.Generic
    open rvinowise.extensions

    open Xunit
    open FsUnit

    let vertices_reacheble_from_vertices
        (is_vertex_needed:Vertex_id->bool)
        (step_further: Vertex_id -> Vertex_id Set)
        (starting_vertices: Vertex_id seq)
        =
        let rec vertices_reacheble_from_vertices
            (is_vertex_needed:Vertex_id->bool)
            (step_further: Vertex_id -> Vertex_id Set)
            (reached_goals: Vertex_id Set)
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
                
                further_vertices
                |>vertices_reacheble_from_vertices
                    is_vertex_needed
                    step_further
                    (
                        reached_goals
                        |>Set.union needed_vertices
                    )
            else
                reached_goals

        vertices_reacheble_from_vertices
            is_vertex_needed
            step_further
            Set.empty
            starting_vertices
    


    let first_vertices 
        (step_further: Vertex_id -> Vertex_id Set)
        (vertices: Vertex_id Set)
        =
        let reached_vertices =
            vertices
            |>Seq.fold(fun set vertex->
                    [vertex]
                    |>vertices_reacheble_from_vertices
                        (fun vertex->vertices|>Set.contains vertex)
                        step_further
                    |>Set.union set
                )
                Set.empty
        reached_vertices
        |>Set.difference vertices

    let first_vertices_reacheble_from_vertices
        (is_vertex_needed: Vertex_id->bool)
        (step_further: Vertex_id -> Vertex_id Set)
        (starting_vertices: Vertex_id seq)
        =
        starting_vertices
        |>Seq.singleton
        |>Seq.map (
            vertices_reacheble_from_vertices 
                is_vertex_needed
                step_further
        )
        |>Set.intersectMany
        |>first_vertices step_further

    
    [<Fact>]
    let ``vertices reacheble from others``()=
        vertices_reacheble_from_vertices
            (fun _->true)
            (Edges.next_vertices example.Figure.a_high_level_relatively_simple_figure.edges)
            (["b0";"b2"]|>List.map Vertex_id|>Set.ofList)
        |> should equal [Vertex_id "f1"]

    [<Fact>]
    let ``vertices reaching others``()=
        vertices_reacheble_from_vertices
            (fun _->true)
            (Edges.previous_vertices example.Figure.a_high_level_relatively_simple_figure.edges)
            (["b1";"f1"]|>List.map Vertex_id|>Set.ofList)
        |> should equal [Vertex_id "b0"]