namespace rvinowise.ai

type Sequence = Figure_id array

module Sequence =

    let to_figure_id 
        (sequence) 
        =
        sequence
        |>Seq.map Figure_id.value
        |>String.concat ""
        |>Figure_id
    
    let ofString 
        (str_sequence:string)
        =
        str_sequence|>Seq.map (string>>Figure_id)|>Array.ofSeq
        