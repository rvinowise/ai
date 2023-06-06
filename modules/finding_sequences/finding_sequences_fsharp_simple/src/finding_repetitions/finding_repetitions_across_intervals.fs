namespace rvinowise.ai

open System


type Interval_with_repetitions = {
    a_appearances: Interval array;
    b_appearances: Interval array;
}

module ``Finding_repetitions_across_intervals(simple)`` =
   

    let repeated_pair_across_intervals 
        (halves_can_form_pair: Interval->Interval->bool)
        (interval1: Interval_with_repetitions)
        (interval2: Interval_with_repetitions)
        =
        let appearances_in_interval1 =
            ``Finding_repetitions(fsharp_simple)``.repeated_pair
                halves_can_form_pair
                interval1.a_appearances
                interval1.b_appearances
        let appearances_in_interval2 =
            ``Finding_repetitions(fsharp_simple)``.repeated_pair
                halves_can_form_pair
                interval2.a_appearances
                interval2.b_appearances
        if 
            appearances_in_interval1.Length = 0
            ||
            appearances_in_interval2.Length = 0
        then
            [||]
        else
            appearances_in_interval2
            |>Array.append appearances_in_interval1

                



