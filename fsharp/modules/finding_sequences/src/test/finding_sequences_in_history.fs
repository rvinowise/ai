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
                []
            ]
            |>ai.Combined_history.built.ofSignals