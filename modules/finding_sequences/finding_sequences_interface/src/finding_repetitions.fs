namespace rvinowise.ai


open FsUnit
open Xunit

module Finding_repetitions =

    let halves_are_close_enough 
        maximum_distance_between_a_and_b
        (a:Interval) (b:Interval)
        =
        let distance_to_b: Moment = 
            b.start - a.finish
        if (distance_to_b <= maximum_distance_between_a_and_b) then
            true
        else
            false
    
    let all_halves (_:Interval) (_:Interval) = true

    let repeated_pair = 
        ``Finding_repetitions(fsharp_simple)``.repeated_pair
    let repeated_pair_of_sequences = 
        ``Finding_repetitions(fsharp_simple)``.repeated_pair_of_sequences


    [<Fact>]
    let ``try repeated_pair_of_sequences``()=
        let a_history =
                [|Figure_id "a"|],
                ([0;5]|>Seq.map Interval.moment|>Array.ofSeq)
            
        let b_history =
                [|Figure_id "b"|],
                ([1;7]|>Seq.map Interval.moment|>Array.ofSeq)
            
        (a_history, b_history)                    
        |>repeated_pair_of_sequences
            all_halves
            
        |>should equal
            (
                [|"a";"b"|]|>Array.map Figure_id
                ,
                [
                    0,1; 5,7
                ]
                |>Seq.map Interval.ofPair
                |>Array.ofSeq
            )

