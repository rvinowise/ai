namespace rvinowise.ai.applying_stencil_impl

module Map_first_nodes =
    open rvinowise.ai.generating_combinations
    open rvinowise.ai

    (*a breaking function, which doesn't waste time on further search, if at least one referenced_figure is lacking.
    a simple optimisation, similar to "break" in C#
    
    works slightly faster generally; 
    but it's 10x slower if a big stencil is failed to be applied to a small figure
    *)
    let subfigures_which_reference_figures
        (target:Figure)
        (referenced_figures: Figure_id seq)
        =
        let rec subfigures_for_next_figure
            (target:Figure)
            (referenced_figures: Figure_id list)
            (subfigures_for_all_figures: (Figure_id*Vertex_id seq) list )
            =
            match referenced_figures with
            |[]->subfigures_for_all_figures
            |referenced_figure::rest_figures->
                let subfigures_in_target =
                    Figure.vertices_referencing_lower_figure target referenced_figure
                if Seq.length subfigures_in_target = 0 then
                    []
                else
                    subfigures_for_next_figure
                        target
                        rest_figures
                        ((referenced_figure,subfigures_in_target)::subfigures_for_all_figures)
            
        let vertices = 
            subfigures_for_next_figure
                target
                (List.ofSeq referenced_figures)
                []
        if Seq.length vertices = 0 then
            [],false
        else
            vertices,true


    let ``map_first_nodes(breaking recursion)``
        (stencil: Stencil)
        target
        =
        let figures_to_map = 
            stencil
            |>Stencil.first_referenced_figures
            
        let first_subfigures_of_stencil = 
            Stencil.first_subfigures stencil

        let subfigures_in_target,target_has_all_subfigures =
            figures_to_map
            |>subfigures_which_reference_figures target
        if target_has_all_subfigures then
            subfigures_in_target
            |>Seq.map (fun (referenced_figure, subfigures_in_target) ->
                referenced_figure
                |>Stencil.vertices_referencing_figure 
                        stencil
                        first_subfigures_of_stencil
                |>Seq.map (fun subfigure_in_stencil->
                    Element_to_targets<Vertex_id,Vertex_id> (subfigure_in_stencil,subfigures_in_target);
                )
                |>Generator_of_mappings<Vertex_id, Vertex_id>
            )
            |>Work_with_generators.mapping_combinations_from_generators
            |>Seq.map Work_with_generators.mapping_from_generator_output
        else
            Seq.empty
            
    
    (* simplest straight-forward implementation. reliably fast *)
    let ``map_first_nodes(checking after full calculation)``
        (stencil: Stencil)
        target
        =
        let figures_to_map = 
            stencil
            |>Stencil.first_referenced_figures
            
        let first_subfigures_of_stencil = 
            Stencil.first_subfigures stencil

        let subfigures_in_target =
            figures_to_map
            |>Seq.map (fun referenced_figure->
                referenced_figure,
                Figure.vertices_referencing_lower_figure target referenced_figure
            )
        let some_figures_are_lacking =  
            subfigures_in_target
            |>Seq.exists(fun pair->pair|>snd|>Seq.isEmpty)
        if some_figures_are_lacking then
            Seq.empty
        else 
            subfigures_in_target
            |>Seq.map (fun (referenced_figure, subfigures_in_target) ->
                referenced_figure
                |>Stencil.vertices_referencing_figure 
                        stencil
                        first_subfigures_of_stencil
                |>Seq.map (fun subfigure_in_stencil->
                    Element_to_targets<Vertex_id,Vertex_id> (subfigure_in_stencil,subfigures_in_target);
                )
                |>Generator_of_mappings<Vertex_id, Vertex_id>
            )
            |>Work_with_generators.mapping_combinations_from_generators
            |>Seq.map Work_with_generators.mapping_from_generator_output