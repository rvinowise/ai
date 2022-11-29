namespace rvinowise.ai.figure

open rvinowise.ai.mapping_stencils
open rvinowise.ai

module Applying_stencil = 

    type Mapped_stencil = {
        subfigures: (Subfigure*Subfigure) Set
    }

    let retrieve_result
        target
        mapping
        =
        ()


    let input_for_first_mappings_permutators
        stencil
        target
        =
        let first_nodes_of_stencil = Figure.first_subfigures stencil
        
        let figures_to_map = 
            first_nodes_of_stencil
            |>Subfigure.referenced_figures

        let nodes_in_target = 
            figures_to_map
            |>Seq.map (Figure.nodes_referencing_lower_figure target)

        let nodes_in_stencil = 
            figures_to_map
            |>Seq.map (fun f->
                Subfigure.referencing_figure f first_nodes_of_stencil
            )
        
        {|
            nodes_in_target = nodes_in_target;
            nodes_in_stencil = nodes_in_stencil
        |}

    let map_first_nodes
        stencil
        target
        =

        
            
        let generator = Generator_of_order_sequences<int[]>();
        ()
        

        // figures_to_map
        // |>Seq.map generator.add_order(Generator_of_mappings(
        //     target
        //     |> Figure.subfigure_occurances 
        //     |> Seq.length, 
        //     Seq.length occurances_in_stencil
        // ))

    let map_stencil_onto_target
        stencil
        target 
        =
        ()


    let results_of_stencil_application
        stencil
        target
        =
        target
        //|>map_stencil_onto_target stencil
        //|>Seq.map retrieve_result target
    
