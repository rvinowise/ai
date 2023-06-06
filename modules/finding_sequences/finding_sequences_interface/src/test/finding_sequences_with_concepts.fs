namespace rvinowise.ai.test

open Xunit
open Xunit.Abstractions
open FsUnit
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open rvinowise.ai


module finding_sequences_with_concepts =
    open rvinowise.ai

    let ``try finding sequences with a number concept``()=
        let number_concept = 
            built.Stencil.simple_with_separator [
                "N","out";
                ",#1","out";
                "out",",#2";
                "out",";";
            ]
        let history =
            ["N1,2,3,4;";"1+2=3;2+2=4"]
    //mom:   012345678   9¹123456789
            |>Event_batches.from_text_blocks
            |>Event_batches.to_sequence_appearances
        
        [number_concept]
        |>Finding_many_repetitions.all_repetitions_with_concepts history
        |>Set.ofSeq
        |>Set.isSubset (
            [
                {
                    Sequence_appearances.sequence=
                        [|"[number]";"+";"[number]";"=";"[number]"|]
                        |>Array.map Figure_node.ofString
                    appearances=
                        [9,13; 15,19]
                        |>Seq.map Interval.ofPair

                };
                {
                    Sequence_appearances.sequence=
                        [|"[number]";"+";"2";"=";"[number]"|]
                        |>Array.map Figure_node.ofString
                    appearances=
                        [9,13; 15,19]
                        |>Seq.map Interval.ofPair

                };
            ]|>Set.ofSeq
        )
        |>should equal true