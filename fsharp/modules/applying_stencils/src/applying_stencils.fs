namespace rvinowise.ai.figure

open System.Diagnostics.Contracts
open rvinowise.ai.mapping_stencils
open rvinowise.ai

module Applying_stencil = 

    type Mapped_stencil = {
        subfigures: (Node_id*Node_id) Set
    }

    
    let sorted_subfigures_to_map_first stencil target =
        let first_subfigures_of_stencil = Stencil.first_subfigures stencil
        
        let figures_to_map = 
            first_subfigures_of_stencil
            |>Subfigures.referenced_figures

        let subfigures_in_target = 
            figures_to_map
            |>Seq.map (Figure.nodes_referencing_lower_figure target)
            |>Seq.map Subfigures.ids
            |>Seq.map Array.ofSeq
            
        let subfigures_in_stencil = 
            figures_to_map
            |>Seq.map (fun f->
                Subfigures.pick_referencing_figure f first_subfigures_of_stencil
            )
            |>Seq.map Subfigures.ids
            
        (figures_to_map, subfigures_in_stencil, subfigures_in_target )


    let input_for_first_mappings_permutators subfigures_in_stencil subfigures_in_target =
        
        let amounts_in_stencil = 
            subfigures_in_stencil
            |>Seq.map Seq.length

        let amounts_in_target = 
            subfigures_in_target
            |>Seq.map Seq.length

        Seq.zip amounts_in_stencil amounts_in_target

           
    let prepared_generator_of_first_mappings
        subfigures_in_stencil
        subfigures_in_target
        =
        let generator = Generator_of_order_sequences<int[]>()
        
        (input_for_first_mappings_permutators subfigures_in_stencil subfigures_in_target)
        |>Seq.map Generator_of_mappings
        |>Seq.iter generator.add_order
        
        generator
        
    
    let mapping_of_subfigure
        (target_subfigures:Node_id[])
        target_subfigure_index
        subfigure_of_stencil
        =
        (subfigure_of_stencil, target_subfigures[target_subfigure_index])
        
    let mappings_of_figure
        used_occurances
        subfigures_chosen_by_stencil
        subfigures_available_in_target
        =
        (used_occurances, subfigures_chosen_by_stencil)
        ||>Seq.map2 (mapping_of_subfigure subfigures_available_in_target)
        
        
    let mapping_from_generator_output
        subfigures_in_stencil
        subfigures_in_target
        indices
        =
        Contract.Requires ((Seq.length subfigures_in_stencil) = (Seq.length subfigures_in_target))
        
        (indices, subfigures_in_stencil, subfigures_in_target)
        |||>Seq.map3 mappings_of_figure
        |>Seq.concat
        |>Set.ofSeq
        
        
    let map_first_nodes
        stencil
        target
        =
        let figures_to_map, subfigures_in_stencil, subfigures_in_target =
            sorted_subfigures_to_map_first stencil target
        
        let generator = (prepared_generator_of_first_mappings subfigures_in_stencil subfigures_in_target)
        
        generator
        |>Seq.map (mapping_from_generator_output subfigures_in_stencil subfigures_in_target)

        


    let prolongate_mapping 
        stencil
        target 
        (mapped_nodes: (Node_id*Node_id)seq )
        =
        mapped_nodes
        //|>Seq.map 


    let map_stencil_onto_target
        stencil
        target 
        =
            
        (map_first_nodes stencil target)
        |>Seq.map (prolongate_mapping stencil target)

        

    let retrieve_result target mapping =
        ()

    let results_of_stencil_application
        stencil
        target
        =
        target
        |>map_stencil_onto_target stencil
        //|>Seq.map (retrieve_result target)
    
