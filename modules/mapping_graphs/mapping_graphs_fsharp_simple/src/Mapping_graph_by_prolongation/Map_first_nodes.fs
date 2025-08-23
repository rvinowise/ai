namespace rvinowise.ai

open rvinowise.ai.generating_combinations
open rvinowise.ai
open rvinowise.ai.mapping_graph_impl
open rvinowise.ai.stencil

module Map_first_nodes =
    
    
    
    
    let all_valid_target_vertices_for_mapping
        mapping_function
        target_figure
        =
        target_figure.targets
        |>Map.filter(fun vertex figure ->
            mapping_function vertex
        )|>Map.keys
         
        
    
    let possible_combinations_of_mapping_vertices
        mapping_id_to_function
        (mappee: Unmapped_figure)
        (target: Constant_figure)
        vertices_to_map
        =
        let suitable_vertices_in_target =
            Figure.referenced_targets mappee.targets vertices_to_map
            |>Seq.map (fun mapping_function_id->
                let mapping_function = mapping_id_to_function mapping_function_id

                mapping_function_id,
                all_valid_target_vertices_for_mapping
                    mapping_function
                    target
            )
        let some_figures_are_lacking =  
            suitable_vertices_in_target
            |>Seq.exists(fun pair->pair|>snd|>Seq.isEmpty)
        if some_figures_are_lacking then
            Seq.empty
        else 
            suitable_vertices_in_target
            |>Seq.map (fun (mapping_function, vertices_in_target) ->
                mappee.targets
                |>Figure.vertices_referencing_target 
                    vertices_to_map
                    mapping_function         
                |>Seq.map (fun vertex_in_mappee->
                    Element_to_targets<Vertex_id,Vertex_id> (vertex_in_mappee,vertices_in_target);
                )
                |>Generator_of_mappings<Vertex_id, Vertex_id>
            )
            |>Work_with_generators.mapping_combinations_from_generators
            
    
    let map_within_other_mapping
        mapping_id_to_function
        (within_mapping: Map<Vertex_id,Vertex_id>)
        (mappee: Unmapped_figure)
        (target: Constant_figure)
        =
        let first_vertices_of_mappee = 
            Figure.first_vertices (mappee :> IFigure) |> Set.ofSeq
        
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
            mapping_id_to_function
            mappee
            target
            not_mapped_vertices
        |>Seq.map(fun mapping ->
            already_mapped_vertices
            |>Map.toSeq
            |>Seq.map Element_to_target
            |>Seq.append mapping
        )
        |>Seq.map Work_with_generators.immutable_mapping_from_generator_output
        
        
            
    let map_first_nodes_with_mutable_mapping
        mapping_id_to_function
        mappee
        (target: Constant_figure)
        =
        //good for long stencils and figures, with many prolongations of the mapping
        mappee
        |>Figure.first_vertices
        |>possible_combinations_of_mapping_vertices
              mapping_id_to_function
              mappee 
              target
        |>Seq.map Work_with_generators.mutable_mapping_from_generator_output
    
    
    
            
    let map_first_nodes_with_immutable_mapping
        mapping_id_to_function
        mappee
        (target: Constant_figure)
        =
        //good for short stencils, with few prolongations
        mappee
        |>Figure.first_vertices
        |>possible_combinations_of_mapping_vertices mapping_id_to_function mappee target
        |>Seq.map Work_with_generators.immutable_mapping_from_generator_output