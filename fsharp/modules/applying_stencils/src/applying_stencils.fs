namespace rvinowise.ai.figure

open rvinowise.ai.mapping_stencils

module Applying_stencil = 

    type Mapped_stencil = {
        subfigures: (Subfigure*Subfigure) Set
    }

    let retrieve_result
        target
        mapping
        =
        ()


    

    let map_first_nodes
        stencil
        target
        =

        let first_nodes_of_stencil = Figure.first_nodes stencil
        

        let figures_to_map = 
            first_nodes_of_stencil
            |>Subfigure.participating_figures

        //let appearances_in_stencil = Subfigure.

        let in_target = 
            figures_to_map
            |>Seq.map (Figure.nodes_referencing_lower_figure target)

        let in_stencil = 
            figures_to_map
            |>Seq.map (Figure.nodes_referencing_lower_figure target)
            
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
    
