namespace rvinowise.ai

open Xunit
open FsUnit

open System
open rvinowise.ai
open rvinowise

module ``Finding_many_repetitions(simple)`` =
    

    type Known_sequences = Set<Figure_id array>


    let private is_this_sequence_already_found 
        (known_sequences: Known_sequences)
        ab_sequence
        =
        known_sequences
        |>Set.contains ab_sequence 

    let many_repetitions
        (sequence_appearances: seq<Sequence_appearances>)
        =
        let known_sequences = 
            sequence_appearances
            |>Seq.map (fun history->history.sequence)
            |>Set.ofSeq
        
        (sequence_appearances,sequence_appearances)
        ||>Seq.allPairs
        |>Seq.fold (
            fun 
                (known_sequences, found_pairs)
                (a_history,b_history) 
                ->
            let ab_sequence = 
                Array.append 
                    a_history.sequence
                    b_history.sequence
            if is_this_sequence_already_found
                known_sequences
                ab_sequence
            then
                known_sequences,found_pairs
            else
                let found_pair = 
                    (a_history.appearances, b_history.appearances)
                    |>Finding_repetitions.repeated_pair_with_histories ab_sequence
                if Appearances.has_repetitions found_pair.appearances then
                    (
                        known_sequences
                        |>Set.add found_pair.sequence
                    ),
                    found_pair::found_pairs
                else
                    known_sequences,found_pairs
            )
            (known_sequences,[])
        |>snd


    let repetitions_in_combined_history
        (event_batches:Event_batches)
        =
        event_batches
        |>built.Event_batches.to_sequence_appearances
        |>many_repetitions
        |>built.Event_batches.from_sequence_appearances
        |>built.Event_batches.add_mood_to_combined_history
           (Event_batches.get_mood_history event_batches)
        |>built.Event_batches.remove_batches_without_actions
    

    let all_repetitions 
        (sequence_appearances: seq<Sequence_appearances>)
        =
        let rec steps_of_finding_repetitions
            (all_sequences: seq<Sequence_appearances>)
            (sequences_of_previous_step: seq<Sequence_appearances>)
            =
            if Seq.isEmpty sequences_of_previous_step then
                all_sequences
            else
                let all_sequences =
                    all_sequences
                    |>Seq.append sequences_of_previous_step
                all_sequences
                |>many_repetitions
                |>steps_of_finding_repetitions all_sequences
                
        steps_of_finding_repetitions
            []
            sequence_appearances

