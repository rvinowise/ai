namespace rvinowise.ai.test

open System.Runtime.InteropServices
open Xunit
open Xunit.Abstractions
open FsUnit
open BenchmarkDotNet.Engines
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open rvinowise.extensions.benchmark


module Finding_repetitions_ingenious_benchmark =
    open rvinowise.ai

    type Finding_repetitions_ingenious_benchmark() =
        
        member val all_repetitions_implementations = [
            {Parameter.value= ``Finding_many_repetitions(fsharp_simple)``.all_repetitions; 
            name="all_repetitions(fsharp_simple)"};
            {Parameter.value= ``Finding_many_repetitions(fsharp_no_dictionary)``.all_repetitions; 
            name="all_repetitions(fsharp_no_dictionary)"};
            // {value= Finding_many_repetitions_csharp_gpu.all_repetitions; 
            // name="all_repetitions(csharp_gpu)"}
        ]

        [<ParamsSource("all_repetitions_implementations")>]
        member val all_repetitions = {
                value=``Finding_many_repetitions(fsharp_simple)``.all_repetitions; 
                name="default"
            } with get, set

        member this.overlaid_sequences_long =
            "a1bc2d31a2ef4bg3c54de6fh5g6h"
    //seq1:  a bc d    ef  g        h
    //seq2:   1  2 3     4    5   6  
    //seq3:          a    b  c  de f  g h         
    //seq4:         1 2     3  4     5 6  
    //mom:   0123456789¹123456789²123456789³
            |>built.Event_batches.from_text
            |>built.Event_batches.to_sequence_appearances

        member this.overlaid_sequences =
            "a1bca2d3bc45d"
    //seq1:  a bc  d
    //seq3:      a   bc  d       
    //mom:   0123456789¹123456789²123456789³
            |>built.Event_batches.from_text
            |>built.Event_batches.to_sequence_appearances

        [<Benchmark>]
        member this.all_repetitions_in_overlaid_sequences()=
            this.overlaid_sequences_long
            |>this.all_repetitions.value
            |>Consumer().Consume
            

    [<Fact(Skip="slow")>] //
    let benchmark()=
        let config = 
            DefaultConfig.Instance.
                WithOptions(ConfigOptions.DisableOptimizationsValidator)

        BenchmarkRunner.Run<Finding_repetitions_ingenious_benchmark>(config) |> ignore


