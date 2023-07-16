namespace rvinowise.ai

open FsUnit
open Xunit
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open rvinowise.ai
open BenchmarkDotNet.Engines

type Benchmarking_modifications_of_history() =
    

    let event_batches = 
        "1234567890"
        |>History_from_text.event_batches_from_text
            (History_from_text.mood_changes_as_words_and_numbers "no" "ok")
        |>Event_batches.only_signals

    [<Benchmark>]
    member _.shifting_appearances_in_time() =
        let initial_appearances = 
            event_batches
            |>Event_batches.event_batches_to_figure_appearances 0

        initial_appearances
        |>Map.map (fun _ appearances ->
                appearances|>Appearances.shift_appearances_in_time 100
        )|>Consumer().Consume
        
    
    [<Benchmark>]
    member _.creating_shifted_appearances_from_batches() =
        event_batches
        |>Event_batches.event_batches_to_figure_appearances 100
        |>Consumer().Consume

    [<Fact>]
    member _.``shifting appearances and creating them shifted give same result``()=
        let appearances_created_shifted = 
            event_batches
            |>Event_batches.event_batches_to_figure_appearances 100

        let initial_appearances = 
            event_batches
            |>Event_batches.event_batches_to_figure_appearances 0

        let appearances_shifted_later = 
            initial_appearances
            |>Map.map (fun _ appearances ->
                    appearances|>Appearances.shift_appearances_in_time 100
            )
            //|>Appearances.shift_sequence_appearances_in_time 100

        appearances_created_shifted
        |>should equal appearances_shifted_later

    [<Fact>]
    member _.run()=
        BenchmarkRunner.Run<Benchmarking_modifications_of_history>(
            DefaultConfig.Instance.
                WithOptions(ConfigOptions.DisableOptimizationsValidator)
        ) 
