namespace rvinowise.ai

open Xunit
open FsUnit

open System
open rvinowise.ai
open rvinowise

module Finding_many_repetitions =
    

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
    
    [<Fact>]
    let ``try finding many_repetitions, in simple combined history``()=
        built.Event_batches.from_contingent_signals 0 [
            ["a"];//0
            ["b"];//1
            ["c"];//2
            ["x"];//3
            ["a"];//4
            ["c"];//5
            ["a"];//6
            ["b"];//7
        ]
        |>built.Event_batches.to_sequence_appearances
        |>many_repetitions
        |>Seq.map built.Sequence_appearances.to_figure_id_appearances
        |>Seq.sort
        |>should equal [
            built.Figure_id_appearances.from_tuples "aa" [
                0,4; 4,6
            ]
            built.Figure_id_appearances.from_tuples "ab" [
                0,1; 6,7
            ]
            built.Figure_id_appearances.from_tuples "ac" [
                0,2; 4,5
            ]
            built.Figure_id_appearances.from_tuples "ca" [
                2,4; 5,6
            ]
        ]
    
    [<Fact>]
    let ``try finding many_repetitions, when duplicating sequences are possible``()=
        let ad_sequence = "ad"|>Seq.map string|>Array.ofSeq
        let a_sequence = "a"|>Seq.map string|>Array.ofSeq
        let da_sequence = "da"|>Seq.map string|>Array.ofSeq
        let ad_appearances = {
            Sequence_appearances.sequence=ad_sequence
            appearances=[|
                0,2; 5,6
            |]|>Array.map Interval.ofPair
        }
        let a_appearances = {
            Sequence_appearances.sequence=a_sequence
            appearances=[|
                0;5;8
            |]|>Array.map Interval.moment
        }
        let da_appearances = {
            Sequence_appearances.sequence=da_sequence
            appearances=[|
                2,5; 6,8
            |]|>Array.map Interval.ofPair
        }
        let ada_appearances =
            [ad_appearances;a_appearances;da_appearances]
            |>many_repetitions
            |>Seq.filter (fun sequence_appearances ->
                sequence_appearances.sequence = ("ada"|>Seq.map string|>Array.ofSeq)
            )
        ada_appearances
        |>List.ofSeq
        |>should haveLength 1

        ada_appearances
        |>Seq.head
        |>(fun appearances -> appearances.appearances)
        |>should equal ([
            0,5; 5,8
        ]|>Seq.map Interval.ofPair)


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

    [<Fact>]
    let ``try finding all_repetitions (several levels of abstraction)``()=
        let ab_history = {
            Sequence_appearances.sequence="ab"|>Seq.map string|>Array.ofSeq
            appearances=[|
                0,2; 6,9
            |]|>Array.map Interval.ofPair
        }
        let ac_history = {
            Sequence_appearances.sequence="ac"|>Seq.map string|>Array.ofSeq
            appearances=[|
                0,4; 6,10
            |]|>Array.map Interval.ofPair
        }
        let bc_history = {
            Sequence_appearances.sequence="bc"|>Seq.map string|>Array.ofSeq
            appearances=[|
                2,4; 9,10
            |]|>Array.map Interval.ofPair
        }
        let abc_history = {
            Sequence_appearances.sequence="abc"|>Seq.map string|>Array.ofSeq
            appearances=[|
                0,4; 6,10
            |]|>Array.map Interval.ofPair
        }
        let expected_sequences = 
            Set.ofList [
                ab_history;ac_history;bc_history;abc_history
            ]
        
        "a1b2c3a45bc"
//       a b c
//             a  bc
//mom:   0123456789¹1
        |>built.Event_batches.from_text
        |>built.Event_batches.to_sequence_appearances
        |>all_repetitions
        |>Set.ofSeq
        |>Set.intersect expected_sequences
        |>should equal expected_sequences

   