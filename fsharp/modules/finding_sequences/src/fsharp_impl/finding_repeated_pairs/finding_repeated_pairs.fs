namespace rvinowise.ai.fsharp_impl

open System
open rvinowise.ai

module Finding_repeated_pairs =

  
    let pair_with_next_unobstructed_b_start
        (a_appearances: Interval array)
        (b_appearances: Interval array)
        (a_finish: Moment)
        =
        let closest_finish = 

        

    let repeated_pair 
        (a_appearances: Interval array)
        (b_appearances: Interval array)
        =

        a_appearances
        |>Seq.map Interval.finish
        |>Seq.map pair_with_next_unobstructed_b_start a_appearances b_appearances


