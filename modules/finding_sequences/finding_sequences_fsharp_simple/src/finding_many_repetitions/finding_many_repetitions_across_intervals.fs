namespace rvinowise.ai

open System


module ``Finding_many_repetitions_across_intervals(simple)`` =

    let take_which_exist_in_other_interval 
        (other_interval: (Sequence*Interval array) seq)
        (this_interval: (Sequence*Interval array) seq)
        =
        this_interval
        |>Seq.filter (fun (sequence,_)->
            other_interval|>Seq.exists (fst>>(=)sequence)
        )
    
    let take_commonalities_between_2_intervals
        (interval1_appearances: (Sequence*Interval array) seq) 
        (interval2_appearances: (Sequence*Interval array) seq)
        =
        let interval1_history = 
            interval1_appearances
            |>take_which_exist_in_other_interval interval2_appearances
        let interval2_history = 
            interval2_appearances
            |>take_which_exist_in_other_interval interval1_appearances
        interval1_history,interval2_history

    let repetitions_in_2_intervals
        halves_can_form_pair
        (interval1_appearances: (Sequence*Interval array) seq) 
        (interval2_appearances: (Sequence*Interval array) seq)
        =
        let history1,
            history2 = 
                take_commonalities_between_2_intervals
                    interval1_appearances
                    interval2_appearances
        ()