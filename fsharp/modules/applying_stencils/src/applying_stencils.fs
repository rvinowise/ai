namespace rvinowise.ai.figure

open System.Diagnostics.Contracts
open rvinowise.ai.mapping_stencils
open rvinowise.ai

module Applying_stencil = 

    type Mapped_stencil = {
        subfigures: (Subfigure*Subfigure) Set
    }

    
    let sorted_subfigures_to_map_first stencil target =
        let first_subfigures_of_stencil = Stencil.first_subfigures stencil
        
        let figures_to_map = 
            first_subfigures_of_stencil
            |>Subfigure.referenced_figures

        let subfigures_in_target = 
            figures_to_map
            |>Seq.map (Figure.nodes_referencing_lower_figure target)

        let subfigures_in_stencil = 
            figures_to_map
            |>Seq.map (fun f->
                Subfigure.pick_referencing_figure f first_subfigures_of_stencil
            )
            
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
        |>Seq.map generator.add_order
        |>ignore
        
        let generator2 = Generator_of_order_sequences<int[]>()
        generator2.add_order(Generator_of_mappings(1, 3));
        generator2.add_order(Generator_of_mappings(1, 1));
        
        for i in generator2 do
            printf "%O" i
        
        generator
        
    let mapping_from_generator_output subfigures_in_stencil subfigures_in_target indices =
        Contract.Requires ((Seq.length subfigures_in_stencil) = (Seq.length subfigures_in_target)) 
        
    let map_first_nodes
        stencil
        target
        =

        let figures_to_map, subfigures_in_stencil, subfigures_in_target =
            sorted_subfigures_to_map_first stencil target
        
        (prepared_generator_of_first_mappings subfigures_in_stencil subfigures_in_target)
        |>Seq.map (mapping_from_generator_output subfigures_in_stencil subfigures_in_target)
        
        

    let map_stencil_onto_target
        stencil
        target 
        =
            
        map_first_nodes stencil target
        

    let retrieve_result target mapping =
        ()

    let results_of_stencil_application
        stencil
        target
        =
        target
        |>map_stencil_onto_target stencil
        |>Seq.map (retrieve_result target)
    
