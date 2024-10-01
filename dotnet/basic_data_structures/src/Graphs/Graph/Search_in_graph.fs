namespace rvinowise.ai


module Search_in_graph=

    let vertices_reacheble_from_any_vertices
        (is_vertex_needed:Vertex_id->bool)
        (is_vertex_blocking_search:Vertex_id->bool)
        (step_further: Vertex_id -> Vertex_id Set)
        (starting_vertices: Vertex_id seq)
        =
        let rec vertices_reacheble_from_vertices
            (reached_goals: Vertex_id Set)
            (starting_vertices: Vertex_id seq)
            =
            let further_vertices =
                starting_vertices
                |>Seq.collect step_further
                |>Set.ofSeq
            
            if Seq.length further_vertices > 0 then
                
                let reached_goals =
                    further_vertices
                    |>Set.filter is_vertex_needed
                    |>Set.union reached_goals
                    
                further_vertices
                |>Seq.tryFind is_vertex_blocking_search
                |>function
                |Some blocking_vertex ->
                    reached_goals
                |None ->
                    further_vertices
                    |>vertices_reacheble_from_vertices
                        reached_goals
            else
                reached_goals

        vertices_reacheble_from_vertices
            Set.empty
            starting_vertices
    


    
    


    

    