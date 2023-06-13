namespace rvinowise.ai

open System
open Xunit
open FsUnit

module ``Finding_repetitions_across_intervals(simple)`` =

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

    type Sequence_findings = {
        previous_largest_sequences: (Sequence*Interval array) seq
        rest_smaller_sequences_found_before: (Sequence*Interval array) seq
    }


    let stage_of_finding_repetitions_in_interval 
        halves_can_form_pair
        findings
        =
        let (
            smaller_sequences,
            largest_sequences
            ) =
            ``Finding_many_repetitions(simple)``.stage_of_finding_repetitions
                halves_can_form_pair
                findings.rest_smaller_sequences_found_before
                findings.previous_largest_sequences
        
        smaller_sequences
        |>Seq.filter (snd>>Seq.isEmpty>>not)
        |>Seq.append findings.previous_largest_sequences
        ,
        largest_sequences
        |>Seq.filter (snd>>Seq.isEmpty>>not)

    let all_sofar_found_sequences_in_interval 
        findings
        =
        findings.previous_largest_sequences
            |>Seq.append findings.rest_smaller_sequences_found_before

    let repetitions_are_depleted
        interval1_findings interval2_findings
        =
        (Seq.isEmpty interval1_findings.previous_largest_sequences
        || 
        Seq.isEmpty interval2_findings.previous_largest_sequences)


    let rec next_step_of_finding_repetitions
        halves_can_form_pair
        (interval1_findings: Sequence_findings)
        (interval2_findings: Sequence_findings)
        =
        let sofar_found_sequences1 =
            interval1_findings
            |>all_sofar_found_sequences_in_interval

        let sofar_found_sequences2 =
            interval2_findings
            |>all_sofar_found_sequences_in_interval

        if 
            repetitions_are_depleted interval1_findings interval2_findings
        then
            sofar_found_sequences1,sofar_found_sequences2
        else

            let (
                    smaller_sequences_interval1,
                    largest_sequences_interval1
                ) 
                = 
                interval1_findings
                |>stage_of_finding_repetitions_in_interval
                    halves_can_form_pair

            let (
                    smaller_sequences_interval2,
                    largest_sequences_interval2
                ) 
                =
                interval2_findings
                |>stage_of_finding_repetitions_in_interval
                    halves_can_form_pair

            let (
                    shared_smaller_sequences_interval1, 
                    shared_smaller_sequences_interval2
                ) 
                = 
                take_commonalities_between_2_intervals 
                    (smaller_sequences_interval1)
                    (smaller_sequences_interval2)
            let (
                    shared_larger_sequences_interval1, 
                    shared_larger_sequences_interval2
                )
                = 
                take_commonalities_between_2_intervals 
                    (largest_sequences_interval1)
                    (largest_sequences_interval2)

            next_step_of_finding_repetitions
                halves_can_form_pair
                {     
                    previous_largest_sequences=shared_larger_sequences_interval1
                    rest_smaller_sequences_found_before = 
                        interval1_findings.rest_smaller_sequences_found_before
                        |>Seq.append shared_smaller_sequences_interval1
                }
                {
                    previous_largest_sequences=shared_larger_sequences_interval2
                    rest_smaller_sequences_found_before = 
                        interval2_findings.rest_smaller_sequences_found_before
                        |>Seq.append shared_smaller_sequences_interval2
                }


    let repetitions_in_2_intervals
        halves_can_form_pair
        (interval1_appearances: (Sequence*Interval array) seq) 
        (interval2_appearances: (Sequence*Interval array) seq)
        =
        next_step_of_finding_repetitions
            halves_can_form_pair
            {
                previous_largest_sequences=interval1_appearances
                rest_smaller_sequences_found_before=[]
            }
            {
                previous_largest_sequences=interval2_appearances
                rest_smaller_sequences_found_before=[]
            }


    

