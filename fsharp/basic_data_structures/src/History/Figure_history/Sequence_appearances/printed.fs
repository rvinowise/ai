module rvinowise.ai.printed.Sequence_appearances

open rvinowise
open rvinowise.ai
open System



let to_string 
    (sequence: Figure_id seq)
    (appearances: Interval seq) =
    let str_sequence= 
        sequence
        |>Seq.map Figure_id.value
        |>String.concat "" 
    let str_appearances = printed.Interval.sequence_to_string appearances
    $"""{str_sequence} appearances={str_appearances}"""

    




