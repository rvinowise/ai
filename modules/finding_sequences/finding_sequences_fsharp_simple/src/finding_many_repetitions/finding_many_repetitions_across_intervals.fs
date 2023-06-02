namespace rvinowise.ai

open System


module ``Finding_many_repetitions_across_intervals(no_dictionary)`` =

    let repeated_pairs_with_histories
        (halves_can_form_pair: Interval->Interval->bool)
        (appearances: (Sequence_history_debug*Sequence_history_debug) seq)
        =
        appearances
        |>Seq.fold (
            fun 
                found_pairs
                (a_history,b_history) 
                ->
            let ab_sequence = 
                Array.append 
                    a_history.sequence_history.sequence
                    b_history.sequence_history.sequence
            let found_pair =
                (a_history.sequence_history.appearances, b_history.sequence_history.appearances)
                |>``Finding_repetitions(fsharp_simple)``.repeated_pair_with_histories
                    halves_can_form_pair
                    ab_sequence
            
            //let str_a = a_history.sequence_history.sequence|>Sequence_printing.sequence_to_string
            //let str_b = b_history.sequence_history.sequence|>Sequence_printing.sequence_to_string
            let found_pair = 
                {
                    Sequence_history_debug.sequence_history=found_pair;
                    remark= ""//$"""{str_a}.{str_b}"""
                }
            if Appearances.has_repetitions found_pair.sequence_history.appearances then
                found_pair::found_pairs
            else
                found_pairs
            )
            []
        


    let stage_of_finding_repetitions
        halves_can_form_pair
        (previous_smaller_sequences: seq<Sequence_history_debug>)
        (previous_largest_sequences: seq<Sequence_history_debug>)
        =
        let largest_sequences =
            (previous_largest_sequences, previous_largest_sequences)
            ||>Seq.allPairs
            |>repeated_pairs_with_histories halves_can_form_pair
        
        let smaller_sequences =
            (previous_smaller_sequences, previous_largest_sequences)
            ||>Seq.allPairs
            |>repeated_pairs_with_histories halves_can_form_pair
        
        smaller_sequences, largest_sequences



    let all_repetitions_with_remark
        (halves_can_form_pair)
        (report_findings: Sequence_history_debug seq -> unit )
        (sequence_appearances: Sequence_appearances seq)
        =
        let rec next_step_of_finding_repetitions
            (all_sequences_found_before: Sequence_history_debug seq)
            (smaller_sequences_of_previous_step: Sequence_history_debug seq)
            (largest_sequences_of_previous_step: Sequence_history_debug seq)
            =
            let sofar_found_sequences =
                largest_sequences_of_previous_step
                |>Seq.append smaller_sequences_of_previous_step
                |>Seq.append all_sequences_found_before
            
            largest_sequences_of_previous_step
            |>Seq.append smaller_sequences_of_previous_step
            |>report_findings

            if Seq.isEmpty largest_sequences_of_previous_step then
                sofar_found_sequences
            else
                let sofar_found_smaller_sequences =
                    smaller_sequences_of_previous_step
                    |>Seq.append all_sequences_found_before

                stage_of_finding_repetitions
                    halves_can_form_pair
                    sofar_found_smaller_sequences
                    largest_sequences_of_previous_step 
                ||>next_step_of_finding_repetitions 
                    sofar_found_sequences
                
        next_step_of_finding_repetitions
            [] []
            (
                sequence_appearances
                |>Seq.map (fun history->
                    {
                        sequence_history=history;
                        remark=""
                    }
                )
            )
    


    let all_repetitions
        halves_can_form_pair
        (report_findings: seq<Sequence_history_debug> ->unit)
        (sequence_appearances: seq<Sequence_appearances>)
        =
        sequence_appearances
        |>all_repetitions_with_remark
            halves_can_form_pair
            report_findings
        |>Seq.map(fun history ->
            history.sequence_history
        )

    let repetitions_across_intervals
        halves_can_form_pair
        (interval1_appearances: Sequence_appearances seq) 
        (interval2_appearances: Sequence_appearances seq)
        =
