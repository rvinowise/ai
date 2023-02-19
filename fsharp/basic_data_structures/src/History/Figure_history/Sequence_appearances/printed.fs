module rvinowise.ai.printed.Sequence_appearances

open rvinowise
open rvinowise.ai
open System



let to_string 
    (sequence: Sequence)
    (appearances: Interval seq) =
    let str_sequence= sequence|>String.concat "" 
    let str_appearances = printed.Interval.sequence_to_string appearances
    $"""{str_sequence} appearances={str_appearances}"""

    




