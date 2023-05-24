namespace rvinowise.ai.test

open System.Runtime.InteropServices
open Xunit
open Xunit.Abstractions
open FsUnit
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running


module ``testing repeated_pair`` =
    open rvinowise.ai


    [<Fact>]
    let ``in tiny intricate sequences``()=
        Finding_repetitions.repeated_pair
            [|
                Interval.from_int 0 1;
                Interval.from_int 2 3;
                Interval.from_int 4 5;
            |]
            [|
                Interval.from_int 0 1;
                Interval.from_int 2 3;
                Interval.from_int 4 5;
            |]
        |> should equal
            [|
                Interval.from_int 0 3 ;
                Interval.from_int 2 5 ;
            |]
    
    [<Fact>]
    let ``in big sequences``()=
        let items_amount = 100
        
        let heads = [|
            for i in 0..items_amount ->
                Interval.from_int i (i+1)
        |]
        let tails = [|
            for i in 0..items_amount ->
                Interval.from_int i (i+1) 
        |]
        
        Finding_repetitions.repeated_pair heads tails
        |>should equal 
            [|
                for i in 0..items_amount-2 ->
                    Interval.from_int i (i+3)
            |]

    [<Fact>]//(Timeout=1000)
    let ``when the last A-figure is taken, but there's still B-figures left ``()=
        async { 
            let signal1 = built.Figure_id_appearances.from_moments "signal1" [0;5]
            let signal2 = built.Figure_id_appearances.from_moments "signal2" [1;6;7]
            Finding_repetitions.repeated_pair
                (signal1.appearances|>Array.ofSeq)
                (signal2.appearances|>Array.ofSeq)
            |>should equal (
                [
                    0,1; 5,6
                ]
                |>Seq.map Interval.ofPair
            )
        }