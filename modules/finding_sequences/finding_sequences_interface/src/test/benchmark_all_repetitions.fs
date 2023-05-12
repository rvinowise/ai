namespace rvinowise.ai.test

open System.Runtime.InteropServices
open Xunit
open Xunit.Abstractions
open FsUnit
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open rvinowise.extensions.benchmark


module ``finding_sequences, benchmark`` =
    open rvinowise.ai

    type Benchmarking_all_repetitions() =
        

        [<Benchmark>]
        member this.repeated_pair_in_big_sequences()=
            Finding_repetitions.repeated_pair this.heads this.heads


        member val all_repetitions = [
            {Parameter.value= ``Finding_many_repetitions(fsharp_simple)``.all_repetitions; 
            name="all_repetitions(fsharp_simple)"};
            {value= ``Finding_many_repetitions(fsharp_gpu)``.all_repetitions; 
            name="all_repetitions(fsharp_gpu)"}
        ]

        [<ParamsSource("all_repetitions")>]
        member val all_repetitions = {
                value=``Finding_many_repetitions(fsharp_simple)``.all_repetitions; 
                name="default"
            } with get, set

        member this.overlaid_sequences =
            "a1bc2d31a2ef4bg3c54de6fh5g6h"
    //seq1:  a bc d    ef  g        h
    //seq2:   1  2 3     4    5   6  
    //seq3:          a    b  c  de f  g h         
    //seq4:         1 2     3  4     5 6  
    //mom:   0123456789¹123456789²123456789³
            |>built.Event_batches.from_text
            |>built.Event_batches.to_sequence_appearances

        [<Benchmark>]
        member this.all_repetitions_in_overlaid_sequences()=
            this.overlaid_sequences
            |>this.all_repetitions
            
        [<Fact(Skip="slow")>] //
        member _.benchmark()=

            let config = 
                DefaultConfig.Instance.
                    WithOptions(ConfigOptions.DisableOptimizationsValidator)

            BenchmarkRunner.Run<Benchmarking_all_repetitions>(config) |> ignore



