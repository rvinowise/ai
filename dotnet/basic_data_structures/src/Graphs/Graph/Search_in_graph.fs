namespace rvinowise.ai


module Search_in_graph=

    
    let rec graph_elements_reacheble_from_elements<'Element when 'Element : comparison>  
        (is_element_needed:'Element->bool)
        (step_further: 'Element -> 'Element Set)
        (reached_goals: 'Element Set)
        (starting_elements: 'Element seq)
        // 
        =
        let further_elements =
            starting_elements
            |>Seq.collect step_further
            |>Set.ofSeq
        
        if Seq.length further_elements > 0 then
            let needed_elements =
                further_elements
                |>Set.filter is_element_needed
            
            further_elements
            |>graph_elements_reacheble_from_elements
                is_element_needed
                step_further
                (
                    reached_goals
                    |>Set.union needed_elements
                )
        else
            reached_goals

    
    let vertices_reacheble_from_any_vertices
        (is_vertex_needed:Vertex_id->bool)
        (step_further: Vertex_id -> Vertex_id Set)
        (starting_vertices: Vertex_id seq)
        =
        graph_elements_reacheble_from_elements
            is_vertex_needed
            step_further
            Set.empty
            starting_vertices
    
    
    
    let further_edges_from_any_vertices
        (is_edge_needed: Edge->bool)
        further_edges_from_vertex
        tip_vertex_of_edge
        (starting_vertices: Vertex_id seq)
        =
        let step_further (edge:Edge) =
            edge
            |>tip_vertex_of_edge
            |>further_edges_from_vertex

        let first_edges =
            starting_vertices
            |>Seq.collect (further_edges_from_vertex)
        
        graph_elements_reacheble_from_elements
            is_edge_needed
            step_further
            Set.empty
            first_edges
    
    let next_edges_reacheble_from_any_vertices
        all_edges
        (is_edge_needed: Edge->bool)
        (starting_vertices: Vertex_id seq)
        =
        further_edges_from_any_vertices
            is_edge_needed
            (Edges.outgoing_edges all_edges)
            Edge.head
            starting_vertices

    
    let previous_edges_reacheble_from_any_vertices
        all_edges
        (is_edge_needed: Edge->bool)
        (starting_vertices: Vertex_id seq)
        =
        further_edges_from_any_vertices
            is_edge_needed
            (Edges.incoming_edges all_edges)
            Edge.tail
            starting_vertices        

    
    let vertices_reacheble_from_vertex
        (is_vertex_needed:Vertex_id->bool)
        (step_further: Vertex_id -> Vertex_id Set)
        (starting_vertex: Vertex_id)
        =
        starting_vertex
        |>Seq.singleton
        |>vertices_reacheble_from_any_vertices
            is_vertex_needed
            step_further
            
    
    let choose_first_vertices 
        (step_further: Vertex_id -> Vertex_id Set)
        (vertices: Vertex_id Set)
        =
        (*the first vertices in this group are those, which can't be reached by iterating over the graph, starting from
        any other vertex from the selected group*) 
        let reached_vertices =
            vertices
            |>Seq.fold(fun set vertex->
                    [vertex]
                    |>vertices_reacheble_from_any_vertices
                        (fun vertex->vertices|>Set.contains vertex)
                        step_further
                    |>Set.union set
                )
                Set.empty
        reached_vertices
        |>Set.difference vertices

    
    let first_vertices_reacheble_from_all_vertices_together
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
        |>Set.intersectMany
        |>choose_first_vertices step_further


    

    