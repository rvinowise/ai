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
        previous_smaller_sequences: (Sequence*Interval array) seq
        previous_largest_sequences: (Sequence*Interval array) seq
        rest_sequences_found_before: (Sequence*Interval array) seq
        //sofar_found_sequences: (Sequence*Interval array) seq
    }


    let stage_of_finding_repetitions_in_interval 
        halves_can_form_pair
        findings
        =
        ``Finding_many_repetitions(simple)``.stage_of_finding_repetitions
            halves_can_form_pair
            (
                findings.rest_sequences_found_before
                |>Seq.append (findings.previous_smaller_sequences)
            )
            findings.previous_largest_sequences

    let all_sofar_found_sequences_in_interval 
        findings
        =
        findings.previous_largest_sequences
            |>Seq.append findings.previous_smaller_sequences
            |>Seq.append findings.rest_sequences_found_before

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

            let sequences1 = 
                interval1_findings
                |>stage_of_finding_repetitions_in_interval
                    halves_can_form_pair

            let sequences2 = 
                interval2_findings
                |>stage_of_finding_repetitions_in_interval
                    halves_can_form_pair

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
                        previous_smaller_sequences=smaller_sequences|>fst
                        previous_largest_sequences=largest_sequences|>fst
                }
                {
                    interval2_findings with 
                        previous_smaller_sequences=smaller_sequences|>snd
                        previous_largest_sequences=largest_sequences|>snd
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
                previous_smaller_sequences=[]
                rest_sequences_found_before=[]
            }
            {
                previous_largest_sequences=interval2_appearances
                previous_smaller_sequences=[]
                rest_sequences_found_before=[]
            }


    

