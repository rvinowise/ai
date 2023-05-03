module rvinowise.ai.built.Sequence_appearances
    open Xunit
    open FsUnit
    open rvinowise.ai

    let from_figure_id_appearances
        (figure_id_appearances: Figure_id_appearances)
        =
        {
            rvinowise.ai.Sequence_appearances.sequence=
                [|figure_id_appearances.figure|]
            appearances=figure_id_appearances.appearances
        }
    
    let to_figure_id_appearances 
        (sequence_appearances:Sequence_appearances) 
        =
        {
            Figure_id_appearances.figure=
                sequence_appearances.sequence
                |>Seq.map Figure_id.value
                |>String.concat ""
                |>Figure_id
            appearances=sequence_appearances.appearances
        }
    
    let from_string_and_pairs 
        (str_sequence:string)
        (pairs:(Moment*Moment) seq)
        =
        {
            Sequence_appearances.sequence=
                str_sequence|>Seq.map (string>>Figure_id)|>Array.ofSeq
            
            appearances=
                pairs
                |>Seq.map Interval.ofPair
                |>Array.ofSeq
        }