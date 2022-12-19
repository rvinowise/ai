namespace rvinowise.ai.test

open System.Runtime.InteropServices
open BenchmarkDotNet.Configs
open Xunit
open Xunit.Abstractions
open FsUnit
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running


module finding_sequences =
    open rvinowise.ai.Finding_repetitions_cpp
    open rvinowise.ai


    type Benchmarking_finding_repetitions() =
        
        [<Params(10, 1000, 10000)>]
        member val items_amount = 0 with get, set
        
        member this.long_sequence_of_input() = 
            [|
                for i in 0..this.items_amount ->
                    Interval(i,i+1)
            |]

        member this.heads = this.long_sequence_of_input()

        [<Benchmark>]
        member this.prepare_long_sequences_of_input()=
            this.long_sequence_of_input()

        [<Benchmark>]
        member this.search_in_big_sequences()=
            Finding_repetitions_cpp.repeated_pairs this.heads this.heads

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
        member _.benchmark()=

            let config = 
                DefaultConfig.Instance.
                    WithOptions(ConfigOptions.DisableOptimizationsValidator)

            BenchmarkRunner.Run<Benchmarking_finding_repetitions>(config) |> ignore