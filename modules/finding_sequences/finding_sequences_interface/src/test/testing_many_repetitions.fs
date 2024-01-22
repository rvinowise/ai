namespace rvinowise.ai

open Xunit
open FsUnit

open System
open rvinowise.ai
open rvinowise
open rvinowise.ai.example

module ``testing repetitions_of_one_stage`` =


    [<Fact>]
    let ``in simple combined history``()=
        [
            ["a"];//0
            ["b"];//1
            ["c"];//2
            ["x"];//3
            ["a"];//4
            ["c"];//5
            ["a"];//6
            ["b"];//7
        ]|>Event_batches.event_history_from_lists
        |>Event_batches.to_sequence_appearances
        |>Finding_many_repetitions.repetitions_of_one_stage
            Finding_repetitions.all_halves
        |>Appearances.sequence_appearances_to_id_appearances
        |>Seq.sort
        |>should equal [
            "aa"|>Figure_id, 
            [|
                0,4; 4,6
            |]|>Array.map Interval.ofPair;
            "ab"|>Figure_id, 
            [|
                0,1; 6,7
            |]|>Array.map Interval.ofPair;
            "ac"|>Figure_id,  [|
                0,2; 4,5
            |]|>Array.map Interval.ofPair;
            "ca"|>Figure_id,  
            [|
                2,4; 5,6
            |]|>Array.map Interval.ofPair;
        ]
    
    


