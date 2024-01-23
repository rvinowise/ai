namespace rvinowise.ai.test

open Xunit
open FsUnit

open rvinowise.ai



module ``testing all_repetitions (several levels of abstraction)`` =


    [<Fact>]
    let ``finding simple sequences in two steps``()=
        let ab_history = (
            "ab"|>Sequence.ofString,
            [|
                0,2; 6,9
            |]|>Array.map Interval.ofPair
        )

        let ac_history = (
            "ac"|>Sequence.ofString,
            [|
                0,4; 6,10
            |]|>Array.map Interval.ofPair
        )
        let bc_history = (
            "bc"|>Sequence.ofString,
            [|
                2,4; 9,10
            |]|>Array.map Interval.ofPair
        )
        let abc_history = (
            "abc"|>Sequence.ofString,
            [|
                0,4; 6,10
            |]|>Array.map Interval.ofPair
        )
        let expected_sequences = 
            Set.ofList [
                ab_history;ac_history;bc_history;abc_history
            ]
        
        "a1b2c3a45bc"
//       a b c
//             a  bc
//mom:   0123456789¹1
        |>History_from_text.event_batches_from_text
            History_from_text.no_mood
        |>Event_batches.only_signals
        |>Event_batches.to_sequence_appearances
        |>Finding_many_repetitions.all_repetitions
            Finding_repetitions.all_halves
            Reporting.dont
        |>Set.ofSeq
        |>Set.intersect expected_sequences
        |>should equal expected_sequences


    [<Fact>]
    let ``finding long overlaid sequences, performance heavy``()=
        let repetitions =
            "a1bc2d31a2ef4bg3c54de6fh5g6h"
    //seq1:  a bc d    ef  g        h
    //seq2:   1  2 3     4    5   6  
    //seq3:          a    b  c  de f  g h         
    //seq4:         1 2     3  4     5 6  
    //mom:   0123456789¹123456789²123456789³
            |>History_from_text.event_batches_from_text
                History_from_text.no_mood
            |>Event_batches.only_signals
            |>Event_batches.to_sequence_appearances
            |>Finding_many_repetitions.all_repetitions
                Finding_repetitions.all_halves
                Reporting.dont
        //should find long sequences
        repetitions
        |>Set.ofSeq
        |>Set.isProperSubset (
            [
                "abcdefgh"|>Sequence.ofString, [|0,23;8,27|]|>Array.map Interval.ofPair;
                "123456"|>Sequence.ofString, [|1,21;7,26|]|>Array.map Interval.ofPair;
            ]|>Set.ofSeq
        ) |>should equal true

        //should not have duplicates
        repetitions
        |>Set.ofSeq
        |>should haveCount (repetitions|>Seq.length)

    
    [<Fact>]
    let ``when duplicating sequences are possible``()=
        let a_appearances = 
            "a"|>Sequence.ofString,
            [|
                0;5;8
            |]|>Array.map Interval.moment
        
        let d_appearances = 
            "d"|>Sequence.ofString,
            [|
                2;6
            |]|>Array.map Interval.moment
        
        
        let ada_appearances =
            [a_appearances;d_appearances]
            |>Finding_many_repetitions.all_repetitions
                (Finding_repetitions.all_halves)
                Reporting.dont
            |>Seq.filter (fun sequence_appearances ->
                fst sequence_appearances = ("ada"|>Seq.map (string>>Figure_id)|>Array.ofSeq)
            )
        ada_appearances
        |>List.ofSeq
        |>should haveLength 1

        ada_appearances
        |>Seq.head
        |>snd
        |>should equal ([
            0,5; 5,8
        ]|>Seq.map Interval.ofPair)


    

    [<Fact>]//(Skip="ui")
    let ``find repetitions in a text file``()=
        let raw_signals =
            "C:/prj/ai/modules/finding_sequences/mathematical_primers.txt"
            |>History_from_text.event_batches_from_textfile
                (History_from_text.mood_changes_as_words_and_numbers "no" "ok")
            |>Event_batches.only_signals
            |>Event_batches.to_sequence_appearances
        
        raw_signals
        |>Finding_many_repetitions.all_repetitions 
            (Finding_repetitions.halves_are_close_enough 1)
            Reporting.dont
            
        
//        raw_signals
//        |>``Finding_many_repetitions(fsharp_simple)``.all_repetitions
//        |>Seq.sort
//        |>Seq.iter (fun appearances->
//            appearances
//            |>output_dict.WriteLine
//        )