namespace rvinowise.ai

open System

type Sequence_history_debug = {
    sequence_history: Sequence_appearances;
    remark: string
} with
    override this.ToString() =
        let str_sequence=
            this.sequence_history.sequence
            |>Sequence_printing.sequence_to_string
            
        let str_appearances = 
            this.sequence_history.appearances
            |>Interval.intervals_to_string 
        $"""{str_sequence} remark={this.remark} appearances={str_appearances}"""
    

module ``Finding_many_repetitions(fsharp_no_dictionary)`` =

    let repeated_pairs_with_histories
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
        (previous_smaller_sequences: seq<Sequence_history_debug>)
        (previous_largest_sequences: seq<Sequence_history_debug>)
        =
        let largest_sequences =
            (previous_largest_sequences,previous_largest_sequences)
            ||>Seq.allPairs
            |>repeated_pairs_with_histories
        
        let smaller_sequences =
            (previous_smaller_sequences,previous_largest_sequences)
            ||>Seq.allPairs
            |>repeated_pairs_with_histories
        
        smaller_sequences,largest_sequences


    let repetitions_of_one_stage appearances = 
        stage_of_finding_repetitions 
            [] 
            appearances
        |>snd

    let all_repetitions_with_remark
        (report_findings: seq<Sequence_history_debug> -> unit )
        (sequence_appearances: seq<Sequence_appearances>)
        =
        let rec next_step_of_finding_repetitions
            (all_sequences_found_before: seq<Sequence_history_debug>)
            (smaller_sequences_of_previous_step: seq<Sequence_history_debug>)
            (largest_sequences_of_previous_step: seq<Sequence_history_debug>)
            =
            if Seq.isEmpty largest_sequences_of_previous_step then
                smaller_sequences_of_previous_step
                |>Seq.append all_sequences_found_before
            else
                let sofar_found_sequences =
                    largest_sequences_of_previous_step
                    |>Seq.append smaller_sequences_of_previous_step
                    |>Seq.append all_sequences_found_before
                
                let sofar_found_smaller_sequences =
                    smaller_sequences_of_previous_step
                    |>Seq.append all_sequences_found_before
                
                report_findings sofar_found_sequences

                stage_of_finding_repetitions
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
        report_findings
        (sequence_appearances: seq<Sequence_appearances>)
        =
        sequence_appearances
        |>all_repetitions_with_remark report_findings
        |>Seq.map(fun history ->
            history.sequence_history
        )
