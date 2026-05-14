namespace rvinowise.ai


module Search_in_graph=

    
    let rec graph_elements_reacheble_from_elements<'Element when 'Element : comparison>  
        (is_element_needed:'Element->bool)
        (step_further: 'Element -> 'Element Set)
        (reached_goals: 'Element Set)
        (starting_elements: 'Element seq)
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
    
    
    
    
    let further_graph_parts_from_any_vertices
        (is_edge_needed: Edge*Vertex_id -> bool )
        further_edges_from_vertex
        tip_vertex_of_edge
        (starting_vertices: Vertex_id seq)
        =
        let step_further (edge:Edge, vertex) =
                
            edge
            |>tip_vertex_of_edge
            |>further_edges_from_vertex
            |>Set.map (fun next_edge ->
                next_edge, tip_vertex_of_edge next_edge    
            )

        let first_elements =
            starting_vertices
            |>Seq.collect further_edges_from_vertex
            |>Seq.map(fun first_edge ->
                first_edge,tip_vertex_of_edge first_edge
            )
        
        let first_vertices =
            first_elements
            |>Seq.map snd 
        
        let found_edges, found_vertices =
            graph_elements_reacheble_from_elements
                is_edge_needed
                step_further
                Set.empty
                first_elements
            |>List.ofSeq
            |>List.unzip
            
        found_edges,
        first_vertices
        |>Set.ofSeq
        |>Set.union (Set.ofList found_vertices)
    
    let next_parts_reacheble_from_any_vertices
        all_edges
        (is_edge_needed: Edge*Vertex_id->bool)
        (starting_vertices: Vertex_id seq)
        =
        further_graph_parts_from_any_vertices
            is_edge_needed
            (Edges.outgoing_edges all_edges)
            Edge.head
            starting_vertices

    
    let previous_parts_reacheble_from_any_vertices
        all_edges
        (is_edge_needed: Edge*Vertex_id->bool)
        (starting_vertices: Vertex_id seq)
        =
        further_graph_parts_from_any_vertices
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


    

    