namespace rvinowise.ai

open Xunit
open FsUnit

open System
open rvinowise.ai
open rvinowise

module ``Finding_many_repetitions, test`` =


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
        |>Finding_many_repetitions.many_repetitions
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
        let ad_sequence = "ad"|>Seq.map (string>>Figure_id)|>Array.ofSeq
        let a_sequence = "a"|>Figure_id|>Array.singleton
        let da_sequence = "da"|>Seq.map (string>>Figure_id)|>Array.ofSeq
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
            |>Finding_many_repetitions.many_repetitions
            |>Seq.filter (fun sequence_appearances ->
                sequence_appearances.sequence = ("ada"|>Seq.map (string>>Figure_id)|>Array.ofSeq)
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




    [<Fact>]
    let ``try finding all_repetitions (several levels of abstraction)``()=
        let ab_history = {
            Sequence_appearances.sequence="ab"|>Seq.map (string>>Figure_id)|>Array.ofSeq
            appearances=[|
                0,2; 6,9
            |]|>Array.map Interval.ofPair
        }
        let ac_history = {
            Sequence_appearances.sequence="ac"|>Seq.map (string>>Figure_id)|>Array.ofSeq
            appearances=[|
                0,4; 6,10
            |]|>Array.map Interval.ofPair
        }
        let bc_history = {
            Sequence_appearances.sequence="bc"|>Seq.map (string>>Figure_id)|>Array.ofSeq
            appearances=[|
                2,4; 9,10
            |]|>Array.map Interval.ofPair
        }
        let abc_history = {
            Sequence_appearances.sequence="abc"|>Seq.map (string>>Figure_id)|>Array.ofSeq
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
        |>Finding_many_repetitions.all_repetitions
        |>Set.ofSeq
        |>Set.intersect expected_sequences
        |>should equal expected_sequences


    [<Fact>]
    let ``finding long overlaid sequences, performance heavy``()=
        let original_signals =
            "a1bc2d31a2ef4bg3c54de6fh5g6h"
    //seq1:  a bc d    ef  g        h
    //seq2:   1  2 3     4    5   6  
    //seq3:          a    b  c  de f  g h         
    //seq4:         1 2     3  4     5 6  
    //mom:   0123456789¹123456789²123456789³
            |>built.Event_batches.from_text
        
        original_signals 
        |>built.Event_batches.to_sequence_appearances
        |>Finding_many_repetitions.all_repetitions
        |>Set.ofSeq
        |>Set.isProperSubset (
            [
                built.Sequence_appearances.from_string_and_pairs "abcdefgh" [0,23;8,27];
                built.Sequence_appearances.from_string_and_pairs "123456" [1,21;7,26];
            ]|>Set.ofSeq
        ) |>should equal true