namespace rvinowise.ai.math

open Xunit
open FsUnit

open rvinowise.ai
open rvinowise
open rvinowise.ui

module Math_primers=


    [<Fact>]
    let ``find sequences in math primers``()=
        let input_primers =
            built.Event_batches.from_text_blocks [
                "N:0,1,2,3,4,5,6,7,8,9;";
                "1+1=2;×"; "1+2=3;×";
                "1+1=";
    //mom:       0123456789¹123456789²1
    //mom(20+):  2345678    9³12345
    //mom(30+):  6789
            ]
            |>built.Event_batches.from_text
            |>built.Event_batches.to_sequence_appearances
            |>Finding_many_repetitions.all_repetitions
            |>Set.ofSeq
            |>Set.isProperSubset (
                [
                    built.Sequence_appearances.from_string_and_pairs "1+1=" [0,23;8,27];
                    built.Sequence_appearances.from_string_and_pairs "1+1=" [1,21;7,26];
                ]|>Set.ofSeq
            ) |>should equal true

    [<Fact>]
    let ``draw training history``()=
        let input_primers =
            built.Event_batches.from_text_blocks [
                "N:0,1,2,3,4,5,6,7,8,9;";
                "1+1=2;×"; "1+2=3;×";
                "1+1=";
            ]
        
        "math inputs"
        |>infrastructure.Graph.empty
        |>infrastructure.Graph.with_filled_vertex "initial signals"
            (ui.painted.History.add_combined_history input_primers)
        |>ui.painted.image.open_image_of_graph

    [<Fact>]
    let ``ai can reply with rote-memorised constant sequences``()=
        built.Event_batches.from_text_blocks [
            "N:0,1,2,3,4,5,6,7,8,9;";
            "1+1=2;×"; "1+2=3;×";
            "1+1=";
        ]
        |>ai.Desiring_future.desired
        |>should equal [
            ("2",";");
            (";","×");
        ]