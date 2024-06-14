namespace rvinowise.ai


module Search_in_graph=

    let vertices_reacheble_from_any_vertices
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
    


    
    


    

    