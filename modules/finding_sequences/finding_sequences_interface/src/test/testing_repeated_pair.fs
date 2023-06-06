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
        Finding_repetitions.repeated_pair (Finding_repetitions.halves_are_close_enough 1)
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
        
        Finding_repetitions.repeated_pair 
            (Finding_repetitions.halves_are_close_enough 1) 
            heads tails
        |>should equal 
            [|
                for i in 0..items_amount-2 ->
                    Interval.from_int i (i+3)
            |]

    [<Fact>]//(Timeout=1000)
    let ``when the last A-figure is taken, but there's still B-figures left ``()=
        async { 
            let signal1 = 
                [|"signal1"|>Figure_id|],
                [|0;5|]|>Array.map Interval.moment
            let signal2 = 
                [|"signal2"|>Figure_id|],
                [|1;6;7|]|>Array.map Interval.moment
            Finding_repetitions.repeated_pair
                //(Finding_repetitions.halves_are_close_enough 1)
                Finding_repetitions.all_halves
                (snd signal1)
                (snd signal2)
            |>should equal (
                [
                    0,1; 5,6
                ]
                |>Seq.map Interval.ofPair
            )
        }