namespace rvinowise.ai


module Appearances=

    let sequence_appearances_to_string 
        (sequence_appearances: Sequence*Interval array)
        =
        let str_sequence=
            sequence_appearances|>fst
            |>Seq.map Figure_id.value
            |>String.concat ""
            
        let str_appearances = 
            sequence_appearances|>snd
            |>Interval.intervals_to_string 
        
        $"appearances={str_appearances}"
        |>(+) $"{str_sequence}"

    let sequence_appearances_to_id_appearances 
        (history: (Sequence*Interval array) seq)
        =
        history
        |>Seq.map (fun (figure, appearances)->
            (Sequence.to_figure_id figure)
            ,
            appearances
        )

    let has_repetitions appearances =
        Seq.length appearances > 1

    let start appearances =
        appearances
        |>Array.head 
        |>Interval.start

    let finish appearances =
        appearances
        |>Array.last 
        |>Interval.finish

    let shift_appearances_in_time
        added_moments
        (appearances: Interval array)
        =
        appearances
        |>Array.map (fun interval->
            Interval.ofPair (
                (interval.start+added_moments),
                (interval.finish+added_moments)
            )
        )
        
    let shift_sequence_appearances_in_time
        added_moments
        (sequence_appearances: Sequence*Interval array)
        =
        sequence_appearances
        |>snd
        |>shift_appearances_in_time added_moments
    
        
    let sequence_to_text (sequence: Figure_id array) =
        sequence
        |>Seq.map Figure_id.value
        |>String.concat ""
      
    let sequences_appearances_to_text_and_tuples
        (sequences_appearances: (Sequence*Interval array) seq)
        =
        sequences_appearances
        |>Seq.map (fun (sequence, appearances)->
            sequence|>sequence_to_text,
            appearances
            |>Seq.map Interval.toPair
            |>List.ofSeq
        )

    let sequence_appearances_to_text_and_tuples
        (sequence_appearances: Sequence*Interval seq)
        =
        let sequence, appearances = sequence_appearances
        
        sequence|>sequence_to_text
        ,
        appearances
        |>Seq.map Interval.toPair
        |>List.ofSeq
     
  


