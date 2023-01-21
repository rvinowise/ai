namespace rvinowise.ai.math

open Xunit
open FsUnit

open rvinowise.ai
open rvinowise
open rvinowise.ui

module Math_primers=


    [<Fact>]
    let input_primers()=
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