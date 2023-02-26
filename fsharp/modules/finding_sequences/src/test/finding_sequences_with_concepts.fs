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
            built.Stencil.simple [
                "numbers:","out1";
                ",","out1";
                "out1",",";
                "out1",";";
            ]
    