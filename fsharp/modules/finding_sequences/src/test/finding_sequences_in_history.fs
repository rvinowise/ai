namespace rvinowise.ai.test

open Xunit
open Xunit.Abstractions
open FsUnit
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running


module finding_sequences_in_history =
    open rvinowise.ai.Finding_repetitions_cpp
    open rvinowise.ai
    open rvinowise.ui

    [<Fact(Skip="ui")>]
    let ``visualising stages of pattern finding``()=
        let signal_history =
            combined_history.built.from_contingent_signals 0 [
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
        
        
        "stages of pattern search"
        |>infrastructure.Graph.empty
        |>infrastructure.Graph.with_filled_vertex "step 1"
            (ui.painted.History.add_combined_history signal_history)
        |>infrastructure.Graph.with_filled_vertex "step 2"
            (ui.painted.History.add_combined_history step2_history)
        |>ui.painted.image.open_image_of_graph

