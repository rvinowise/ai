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

    type Findings_of_previous_step = {
        all_sequences_found_before: (Sequence*Interval array) seq
        smaller_sequences: (Sequence*Interval array) seq
        largest_sequences: (Sequence*Interval array) seq
        sofar_found_sequences: (Sequence*Interval array) seq
    }

    let rec next_step_of_finding_repetitions
        halves_can_form_pair
        (interval1_findings: Findings_of_previous_step)
        (interval2_findings: Findings_of_previous_step)
        =
        let sofar_found_sequences1 =
            interval1_findings.largest_sequences
            |>Seq.append interval1_findings.smaller_sequences
            |>Seq.append interval1_findings.all_sequences_found_before

        let sofar_found_sequences2 =
            interval2_findings.largest_sequences
            |>Seq.append interval2_findings.smaller_sequences
            |>Seq.append interval2_findings.all_sequences_found_before

        let sequences1 = 
            ``Finding_many_repetitions(simple)``.stage_of_finding_repetitions
                halves_can_form_pair
                (
                    sofar_found_sequences1
                    |>Seq.append interval1_findings.smaller_sequences
                )
                largest_sequences_of_previous_step

        let sequences2 = 
            ``Finding_many_repetitions(simple)``.stage_of_finding_repetitions
                halves_can_form_pair
                sofar_found_smaller_sequences
                largest_sequences_of_previous_step

        let smaller_sequences = 
            take_commonalities_between_2_intervals 
                (sequences1|>fst)
                (sequences2|>fst)
        let largest_sequences = 
            take_commonalities_between_2_intervals 
                (sequences1|>snd)
                (sequences2|>snd)

        next_step_of_finding_repetitions
            halves_can_form_pair
            {
                interval1_findings with 
                    smaller_sequences=smaller_sequences|>fst
                    largest_sequences=largest_sequences|>fst
            }
            {
                interval2_findings with 
                    smaller_sequences=smaller_sequences|>snd
                    largest_sequences=largest_sequences|>snd
            }

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
        
        history2
        |>Seq.allPairs history1
        |>

        