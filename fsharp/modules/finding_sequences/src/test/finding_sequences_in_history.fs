namespace rvinowise.ai.test

open Xunit
open Xunit.Abstractions
open FsUnit
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running


module finding_sequences_in_history =
    open rvinowise.ai.fsharp_impl
    open rvinowise.ai
    open rvinowise.ui

    [<Fact(Skip="ui")>] //
    let ``visualising stages of pattern finding``()=
        let signal_history =
            built.Event_batches.from_contingent_signals 0 [
                ["a";"b"];
                ["c"];
                ["d";"b"];
                ["e"];
                ["f"];
                ["a"];
                ["d";"e"];
                ["e";"f";"g"];
                ["a";"c"];
            ]
        let step2_history =
            signal_history
            |>Finding_many_repetitions.repetitions_in_combined_history
        
        let step3_history =
            step2_history
            |>Finding_many_repetitions.repetitions_in_combined_history

        let step4_history =
            step3_history
            |>Finding_many_repetitions.repetitions_in_combined_history

        "stages of pattern search"
        |>infrastructure.Graph.empty
        |>infrastructure.Graph.with_filled_vertex "step 1"
            (ui.painted.History.add_combined_history signal_history)
        |>infrastructure.Graph.with_filled_vertex "step 2"
            (ui.painted.History.add_combined_history step2_history)
        |>infrastructure.Graph.with_filled_vertex "step 3"
            (ui.painted.History.add_combined_history step3_history)
        |>infrastructure.Graph.with_filled_vertex "step 4"
            (ui.painted.History.add_combined_history step4_history)
        |>ui.painted.image.open_image_of_graph



    [<Fact(Skip="ui")>] //
    let ``visualising stages of pattern finding, including all previous stages``()=
        let signal_history =
            built.Event_batches.from_contingent_signals 0 [
                ["a";"b"];
                ["c"];
                ["d";"b"];
                ["e"];
                ["f"];
                ["a"];
                ["d";"e"];
                ["e";"f";"g"];
                ["a";"c"];
            ]
        let step2_figure_histories =
            signal_history
            |>built.Event_batches.to_separate_histories
            |>Separate_histories.figure_histories
            |>Finding_many_repetitions.many_repetitions
        let step2_combined_history =
            signal_history
            |>built.Event_batches.add_figure_histories step2_figure_histories

        let step3_figure_histories =
            step2_figure_histories
            |>Finding_many_repetitions.many_repetitions
        let step3_combined_history =
            step2_combined_history
            |>built.Event_batches.add_figure_histories step3_figure_histories

        let step4_figure_histories =
            step3_figure_histories
            |>Finding_many_repetitions.many_repetitions
        let step4_combined_history =
            step3_combined_history
            |>built.Event_batches.add_figure_histories step4_figure_histories

        "stages of pattern search with previous stages"
        |>infrastructure.Graph.empty
        |>infrastructure.Graph.with_filled_vertex "step 1"
            (ui.painted.History.add_combined_history signal_history)
        |>infrastructure.Graph.with_filled_vertex "step 2"
            (ui.painted.History.add_combined_history step2_combined_history)
        |>infrastructure.Graph.with_filled_vertex "step 3"
            (ui.painted.History.add_combined_history step3_combined_history)
        |>infrastructure.Graph.with_filled_vertex "step 4"
            (ui.painted.History.add_combined_history step4_combined_history)
        |>ui.painted.image.open_image_of_graph

    [<Fact>] //(Skip="ui")
    let ``visualising stages of pattern finding, with mood``()=
        let signal_history =
            built.Event_batches.from_contingent_signals 0 [
                ["a";"b"];
                ["c"];
                ["+1";"d";"b"];
                ["e"];
                ["+2";"f"];
                ["a"];
                ["d";"e"];
                ["-3";"e";"f";"g"];
                ["a";"c"];
            ]
        let step2_history =
            signal_history
            |>Finding_many_repetitions.repetitions_in_combined_history
        
        let step3_history =
            step2_history
            |>Finding_many_repetitions.repetitions_in_combined_history

        let step4_history =
            step3_history
            |>Finding_many_repetitions.repetitions_in_combined_history

        "stages of pattern search, with mood"
        |>infrastructure.Graph.empty
        |>infrastructure.Graph.with_filled_vertex "step 1"
            (ui.painted.History.add_combined_history signal_history)
        |>infrastructure.Graph.with_filled_vertex "step 2"
            (ui.painted.History.add_combined_history step2_history)
        |>infrastructure.Graph.with_filled_vertex "step 3"
            (ui.painted.History.add_combined_history step3_history)
        |>infrastructure.Graph.with_filled_vertex "step 4"
            (ui.painted.History.add_combined_history step4_history)
        |>ui.painted.image.open_image_of_graph