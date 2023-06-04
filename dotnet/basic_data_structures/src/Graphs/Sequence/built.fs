module rvinowise.ai.built.Sequence
    open Xunit
    open FsUnit
    open rvinowise.ai


    
    let to_figure_id 
        (sequence) 
        =
        sequence
        |>Seq.map Figure_id.value
        |>String.concat ""
        |>Figure_id
    
    let from_string 
        (str_sequence:string)
        (pairs:(Moment*Moment) seq)
        =
        str_sequence|>Seq.map (string>>Figure_id)|>Array.ofSeq
            
        // appearances=
        //     pairs
        //     |>Seq.map Interval.ofPair
        //     |>Array.ofSeq