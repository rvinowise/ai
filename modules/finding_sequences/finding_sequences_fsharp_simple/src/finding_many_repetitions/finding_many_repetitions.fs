namespace rvinowise.ai

open System




module ``Finding_many_repetitions(simple)`` =



    let combinations_of_repeated_pairs
        halves_can_form_pair
        (a_histories: (Sequence*Interval array) seq)
        (b_histories: (Sequence*Interval array) seq)
        =
        (a_histories,b_histories)
        ||>Seq.allPairs
        |>Seq.map (
            ``Finding_repetitions(fsharp_simple)``.repeated_pair_of_sequences 
                halves_can_form_pair
        )

    let stage_of_finding_repetitions
        halves_can_form_pair
        (previous_smaller_sequences: (Sequence*Interval array) seq)
        (previous_largest_sequences: (Sequence*Interval array) seq)
        =
        let largest_sequences =
            (previous_largest_sequences, previous_largest_sequences)
            ||>combinations_of_repeated_pairs halves_can_form_pair
            
        let smaller_sequences =
            (previous_smaller_sequences, previous_largest_sequences)
            ||>combinations_of_repeated_pairs halves_can_form_pair
        
        smaller_sequences, largest_sequences

    

    let repetitions_of_one_stage 
        halves_can_form_pair
        appearances
        = 
        appearances
        |>stage_of_finding_repetitions 
            halves_can_form_pair
            [] 
        |>snd
        |>Seq.filter (snd>>Appearances.has_repetitions)
        

    let all_repetitions
        (halves_can_form_pair)
        (report_findings: (Sequence*Interval array) seq -> unit )
        (sequence_appearances: (Sequence*Interval array) seq)
        =
        let rec next_step_of_finding_repetitions
            (all_sequences_found_before: (Sequence*Interval array) seq)
            (smaller_sequences_of_previous_step: (Sequence*Interval array) seq)
            (largest_sequences_of_previous_step: (Sequence*Interval array) seq)
            =
            let smaller_sequences_of_previous_step = 
                smaller_sequences_of_previous_step
                |>Seq.filter (snd>>Appearances.has_repetitions)
            let largest_sequences_of_previous_step = 
                largest_sequences_of_previous_step
                |>Seq.filter (snd>>Appearances.has_repetitions)

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
            [] [] sequence_appearances
                
    



    
