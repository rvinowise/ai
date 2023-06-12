namespace rvinowise.ai.test

open Xunit
open BenchmarkDotNet.Engines
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open rvinowise.extensions.benchmark
open rvinowise.ai
open rvinowise


type Finding_repetitions_ingenious_benchmark() =
    

    member val all_repetitions_implementations =    
        [
            {
                Parameter.value= 
                    Finding_many_repetitions.all_repetitions
                        (Finding_repetitions.halves_are_close_enough 1)
                        (Reporting.dont);
                name="all_repetitions"
            };
            {
                Parameter.value= 
                    Finding_many_repetitions.all_repetitions
                        (Finding_repetitions.halves_are_close_enough 100)
                        (Reporting.dont);
                name="all_repetitions100"
            };
        ]

    [<ParamsSource("all_repetitions_implementations")>]
    member val all_repetitions = {
            Parameter.value= 
                Finding_many_repetitions.all_repetitions
                    (Finding_repetitions.halves_are_close_enough 1)
                    (Reporting.dont);
            name="all_repetitions"
        }
        with get,set

    member this.long_overlaid_sequences =
        "a1bc2d31a2ef4bg3c54de6fh5g6h"
//seq1:  a bc d    ef  g        h
//seq2:   1  2 3     4    5   6  
//seq3:          a    b  c  de f  g h         
//seq4:         1 2     3  4     5 6  
//mom:   0123456789¹123456789²123456789³
        |>built_from_text.Event_batches.event_batches_from_text
        |>Event_batches.only_signals
        |>Event_batches.event_batches_to_figure_appearances 0
        |>extensions.Map.toPairs
        |>Seq.map (fun (figure, appearances)->
            [|figure|],appearances
        )

    member this.overlaid_sequences =
        "a1bca2d3bc45d"
//seq1:  a bc  d
//seq3:      a   bc  d       
//mom:   0123456789¹123456789²123456789³
        |>built_from_text.Event_batches.event_batches_from_text
        |>Event_batches.only_signals
        |>Event_batches.to_sequence_appearances

    [<Benchmark>]
    member this.all_repetitions_in_overlaid_sequences()=
        this.long_overlaid_sequences
        
        |>this.all_repetitions.value 
        |>Consumer().Consume
        

    [<Fact(Skip="slow")>] //
    member this.benchmark()=
        let config = 
            DefaultConfig.Instance.
                WithOptions(ConfigOptions.DisableOptimizationsValidator)

        BenchmarkRunner.Run<Finding_repetitions_ingenious_benchmark>(config) |> ignore



