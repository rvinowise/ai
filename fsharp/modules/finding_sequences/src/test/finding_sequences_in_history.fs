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

    [<Fact>]
    let ``visualising stages of pattern finding``()=
        let signal_history =
            [
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
            |>ai.Combined_history.built.from_figure_and_mood_histories 0
        signal_history
        |>ui.painted.History.as_graph
        |>ui.painted.image.open_image_of_graph

