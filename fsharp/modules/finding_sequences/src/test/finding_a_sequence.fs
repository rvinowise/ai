namespace rvinowise.ai.test

open System.Runtime.InteropServices
open Xunit
open Xunit.Abstractions
open FsUnit
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running


module ``finding sequences``=
    open rvinowise.ai.Finding_sequences
    open rvinowise.ai


    type ``benchmarking finding repetitions``() =
        
        let items_amount = 10000 
        
        let ``long sequences of input``()=
            let heads = [|
                for i in 0..items_amount ->
                    Interval(i,i+1)
                |]
            let tails = [|
                for i in 0..items_amount ->
                    Interval(i,i+1)
                |]
            (heads, tails)

        let heads, tails = ``long sequences of input``()

        [<Benchmark>]
        member _.``prepare long sequences of input``()=
            ``long sequences of input``()

        [<Benchmark>]
        member _.``search in big sequences``()=
            Finding_sequences.repeated_pairs heads tails

    type ``invoking native methods``(output: ITestOutputHelper)=
        let output = output

        [<Fact>]
        member this.``passing array of structures to a native method``()=
            repeated_pairs
                [|
                    Interval(0,1);
                    Interval(2,3);
                    Interval(4,5);
                |]
                [|
                    Interval(0,1);
                    Interval(2,3);
                    Interval(4,5);
                |]
            |> should equal
                [|
                    Interval(0,3);
                    Interval(2,5);
                |]
            
            ()

        

        [<Fact>]
        member this.``finding repeated pairs in big sequences``()=
            let items_amount = 100
            
            let heads = [|
                for i in 0..items_amount ->
                    Interval(i,i+1)
            |]
            let tails = [|
                for i in 0..items_amount ->
                    Interval(i,i+1)
            |]
            
            repeated_pairs heads tails
            |>should equal 
                [|
                    for i in 0..items_amount-2 ->
                        Interval(i,i+3)
                |]
            
        [<Fact>]
        member _.benchmarking()=

            BenchmarkRunner.Run<``benchmarking finding repetitions``>() |> ignore