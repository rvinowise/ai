namespace rvinowise.ai.test

open Xunit
open Xunit.Abstractions
open FsUnit
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open rvinowise.ai

type Sequence =  array

module finding_sequences_with_concepts =
    open rvinowise.ai

    let ``try finding sequences with a number concept``()=
        let number_concept = 
            built.Stencil.simple [
                "N","out";
                ",1","out";
                "out",",2";
                "out",";";
            ]
        let history =
            "N1,2,3,4,5,6;";
            "1+2=3;2+2=4"
            |>built.Event_batches.from_text_blocks
            |>built.Event_batches.to_sequence_appearances
        Finding_many_repetitions.all_repetitions_with_concepts
            history
            [number_concept]
        |>should 