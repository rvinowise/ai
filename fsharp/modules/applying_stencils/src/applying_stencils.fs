namespace rvinowise.ai

open rvinowise.ai.figure


module Applying_stencils = 

    type Mapped_stencil = {
        subfigures: (Subfigure*Subfigure) Set
    }

    let retrieve_result
        target
        mapping
        =
        ()

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
        |>map_stencil_onto_target stencil
        |>Seq.map retrieve_result target
    
