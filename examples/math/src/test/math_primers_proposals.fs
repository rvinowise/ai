namespace rvinowise.ai.math

open Xunit
open FsUnit
open System.IO

open rvinowise.ai
open rvinowise
open rvinowise.ui

module math_primers_proposals=


    [<Fact(Skip="not implemented")>]
    let ``ai can reply with rote-memorised constant sequences``()=
        History_from_text.event_batches_from_text_blocks [
            "N:0,1,2,3,4,5,6,7,8,9;";
            "1+1=2;×"; "1+2=3;×";
            "1+1=";
        ]
        |>Desiring_future.desired
        |>should equal [
            ("2",";");
            (";","×");
        ]
