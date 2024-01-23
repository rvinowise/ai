namespace rvinowise.ai.mapping_graph_impl

open rvinowise.ai.generating_combinations
open rvinowise.ai


module Map_first_nodes =
    
    let map_first_nodes
        (mappee: Figure)
        (target: Figure)
        =
        let figures_to_map = 
            mappee
            |>Figure.first_referenced_figures
            
        let first_subfigures_of_mappee = 
            Figure.first_vertices mappee

        let subfigures_in_target =
            figures_to_map
            |>Seq.map (fun referenced_figure->
                referenced_figure,
                Figure.all_vertices_referencing_figure referenced_figure target
            )
        let some_figures_are_lacking =  
            subfigures_in_target
            |>Seq.exists(fun pair->pair|>snd|>Seq.isEmpty)
        if some_figures_are_lacking then
            Seq.empty
        else 
            subfigures_in_target
            |>Seq.map (fun (referenced_figure, subfigures_in_target) ->
                mappee
                |>Figure.vertices_referencing_figure 
                    first_subfigures_of_mappee
                    referenced_figure         
                |>Seq.map (fun subfigure_in_mappee->
                    Element_to_targets<Vertex_id,Vertex_id> (subfigure_in_mappee,subfigures_in_target);
                )
                |>Generator_of_mappings<Vertex_id, Vertex_id>
            )
            |>Work_with_generators.mapping_combinations_from_generators
            |>Seq.map Work_with_generators.mapping_from_generator_output