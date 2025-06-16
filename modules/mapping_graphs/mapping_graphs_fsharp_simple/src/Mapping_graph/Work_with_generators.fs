namespace rvinowise.ai.mapping_graph_impl

open rvinowise.ai.generating_combinations
open rvinowise.ai
open rvinowise.ai.stencil

module Work_with_generators =
    
    
    let mapping_from_generator_output
        (chosen_targets: seq<Element_to_target<Vertex_id,Vertex_id>>)
        =
        let mapping = Mapping.empty()
        
        chosen_targets
        |>Seq.iter (fun pair ->
            mapping.Add(pair.element, pair.target)    
        )
        mapping
        
    let mapping_combinations_from_generators
        (generators: Generator_of_mappings<Vertex_id, Vertex_id> seq)
        =
        generators
        //all_generators vertices_of_generator(for_one_figure)     iterations_of_generator
        |>Seq.cast<     Element_to_target<Vertex_id,Vertex_id>seq seq>
        //                    type_of_every_order(digit)
        |>Generator_of_orders<seq<Element_to_target<Vertex_id, Vertex_id>>>
        |>Seq.map (Seq.collect id)
    
            