namespace rvinowise.ai

open rvinowise.ai.generating_combinations
open rvinowise.ai
open rvinowise.ai.mapping_graph_impl

module Map_first_nodes =
    
    
    let possible_combinations_of_mapping_vertices
        (mappee: Figure)
        (target: Figure)
        vertices_to_map
        =
        let suitable_vertices_in_target =
            Figure.referenced_figures mappee vertices_to_map
            |>Seq.map (fun mapped_subfigure->
                mapped_subfigure,
                Figure.all_vertices_referencing_figure mapped_subfigure target
            )
        let some_figures_are_lacking =  
            suitable_vertices_in_target
            |>Seq.exists(fun pair->pair|>snd|>Seq.isEmpty)
        if some_figures_are_lacking then
            Seq.empty
        else 
            suitable_vertices_in_target
            |>Seq.map (fun (referenced_figure, vertices_in_target) ->
                mappee
                |>Figure.vertices_referencing_figure 
                    vertices_to_map
                    referenced_figure         
                |>Seq.map (fun vertex_in_mappee->
                    Element_to_targets<Vertex_id,Vertex_id> (vertex_in_mappee,vertices_in_target);
                )
                |>Generator_of_mappings<Vertex_id, Vertex_id>
            )
            |>Work_with_generators.mapping_combinations_from_generators
            
    
    let map_within_other_mapping
        (mappee: Figure)
        (target: Figure)
        (within_mapping: Map<Vertex_id,Vertex_id>)
        =
        let first_vertices_of_mappee = 
            Figure.first_vertices mappee |>Set.ofSeq
        
        let already_mapped_vertices =
            first_vertices_of_mappee
            |>Seq.map(fun mappee_vertex ->
                Map.tryFind mappee_vertex within_mapping
                |>function
                |Some mapped_vertex ->  Some(mappee_vertex,mapped_vertex) 
                |None -> None
            )
            |>Seq.choose id
            |>Map.ofSeq
        
        let not_mapped_vertices =
            already_mapped_vertices
            |>Map.keys
            |>Set.ofSeq
            |>Set.difference first_vertices_of_mappee
        
        
        possible_combinations_of_mapping_vertices
            mappee target not_mapped_vertices
        |>Seq.map(fun mapping ->
            already_mapped_vertices
            |>Map.toSeq
            |>Seq.map Element_to_target
            |>Seq.append mapping
        )
        |>Seq.map Work_with_generators.immutable_mapping_from_generator_output
        
        
            
    let map_first_nodes_with_mutable_mapping
        (mappee: Figure)
        (target: Figure)
        =
        //good for long stencils and figures, with many prolongations of the mapping
        mappee
        |>Figure.first_vertices
        |>possible_combinations_of_mapping_vertices mappee target
        |>Seq.map Work_with_generators.mutable_mapping_from_generator_output
    
    
    
            
    let map_first_nodes_with_immutable_mapping
        (mappee: Figure)
        (target: Figure)
        =
        //good for short stencils, with few prolongations
        mappee
        |>Figure.first_vertices
        |>possible_combinations_of_mapping_vertices mappee target
        |>Seq.map Work_with_generators.immutable_mapping_from_generator_output