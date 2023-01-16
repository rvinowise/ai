namespace rvinowise.ai.test

open System.Runtime.InteropServices
open Xunit
open Xunit.Abstractions
open FsUnit
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running


module finding_sequences =
    //open rvinowise.ai.cpp_impl
    open rvinowise.ai


    type Benchmarking_finding_repetitions() =
        
        [<Params(5, 50(*, 10000*))>]
        member val items_amount = 0 with get, set
        
        member this.long_sequence_of_input() = 
            [|
                for i in 0..this.items_amount -> 
                    Interval.from_int i (i+1)
            |]

        member this.heads = this.long_sequence_of_input()

        [<Benchmark>]
        member this.prepare_long_sequences_of_input()=
            this.long_sequence_of_input()

        [<Benchmark>]
        member this.cpp_search_in_big_sequences()=
            cpp_impl.Finding_repetitions.repeated_pair this.heads this.heads
        [<Benchmark>]
        member this.fsharp_search_in_big_sequences()=
            fsharp_impl.Finding_repetitions.repeated_pair this.heads this.heads

    type ``invoking native methods``(output: ITestOutputHelper)=
        let output = output

        [<Fact>]
        member this.``f# finding repeated pair in tiny intricate sequences``()=
            fsharp_impl.Finding_repetitions.repeated_pair
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
        member this.``c++ finding repeated pair in tiny intricate sequences``()=
            cpp_impl.Finding_repetitions.repeated_pair
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
        member this.``f# finding repeated pairs in big sequences``()=
            let items_amount = 100
            
            let heads = [|
                for i in 0..items_amount ->
                    Interval.from_int i (i+1)
            |]
            let tails = [|
                for i in 0..items_amount ->
                    Interval.from_int i (i+1) 
            |]
            
            fsharp_impl.Finding_repetitions.repeated_pair heads tails
            |>should equal 
                [|
                    for i in 0..items_amount-2 ->
                        Interval.from_int i (i+3)
                |]
        
        [<Fact>]
        member this.``c++ finding repeated pairs in big sequences``()=
            let items_amount = 100
            
            let heads = [|
                for i in 0..items_amount ->
                    Interval.from_int i (i+1)
            |]
            let tails = [|
                for i in 0..items_amount ->
                    Interval.from_int i (i+1) 
            |]
            
            cpp_impl.Finding_repetitions.repeated_pair heads tails
            |>should equal 
                [|
                    for i in 0..items_amount-2 ->
                        Interval.from_int i (i+3)
                |]
            
        [<Fact>] //(Skip="slow")
        member _.benchmark()=

            let config = 
                DefaultConfig.Instance.
                    WithOptions(ConfigOptions.DisableOptimizationsValidator)

            BenchmarkRunner.Run<Benchmarking_finding_repetitions>(config) |> ignore