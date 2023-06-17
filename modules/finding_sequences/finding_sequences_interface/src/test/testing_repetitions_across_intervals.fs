module rvinowise.ai.``testing repetitions across intervals``

    open Xunit
    open FsUnit
    open rvinowise.ai


    let all_halves (_:Interval) (_:Interval) = true

    let sequence_from_text text =
        text
        |>Seq.map (string>>Figure_id)|>Array.ofSeq

    

    

    [<Fact>]
    let ``try repetitions_in_2_intervals``()=
        let interval1 =
            "1abcdabcd2"
    //       1        2  //noticed, because appeared in 2nd
    //        abcd       //ignored, because didn't appear in 2nd
    //            abcd
    //        ab  ab     //repeated in 2nd
    //mom:   0123456789¹
            |>built_from_text.Event_batches.event_batches_from_text
                built_from_text.Event_batches.no_mood
            |>Event_batches.only_signals
            |>Event_batches.to_sequence_appearances
        let interval2 =
            "d3c1a2b1a2b434"
    //          1 2 1 2      //noticed (appeared in 1st)
    //           a b a b     //noticed (also repeated in 1st)
    //        3         4    //ignored (didn't appear in 1st)
    //                   34
    //mom:   0123456789¹12345        
            |>built_from_text.Event_batches.event_batches_from_text
                built_from_text.Event_batches.no_mood
            |>Event_batches.only_signals
            |>Event_batches.to_sequence_appearances
        
        let interval1_findings,
            interval2_findings =
                Finding_many_repetitions.repetitions_in_2_intervals
                    all_halves
                    interval1
                    interval2
        interval1_findings
        |>Appearances.sequence_appearances_to_text_and_tuples
        |>Set.ofSeq
        |>Set.isProperSubset (
            [
                "12", [0,9];
                "ab", [1,2;5,6];
            ]|>Set.ofList
        )
        |>should equal true
        
        interval2_findings
        |>Appearances.sequence_appearances_to_text_and_tuples
        |>Set.ofSeq 
        |>Set.isProperSubset (
            [
                "12", [3,5;7,9];
                "ab", [4,6;8,10];
            ]|>Set.ofList
        )|>should equal true
