namespace rvinowise.ai

open Xunit
open FsUnit

open System
open rvinowise.ai
open rvinowise

module Finding_repetitions_with_concepts =

    open BenchmarkDotNet.Configs
    open BenchmarkDotNet.Attributes
    open BenchmarkDotNet.Running
    open BenchmarkDotNet.Engines
    

    
    (* 
    Sequence: all subfigures are signals (taking one moment), no parallel signals (e.g. math primers)

    Figure: subfigures have a span (from-to), and can overlap each other
    *)

    let all_repetitions_with_concepts
        (full_history: Figure_id seq)
        (known_concepts: Stencil seq)
        =
        let concept_appearances = 
            known_concepts
            |>Seq.map (
                full_history
                |>built.Figure.from_sequence
                |>Applying_stencil.results_of_stencil_application 
                    
            )
        
        let rec steps_of_finding_repetitions
            (all_sequences: seq<Sequence_appearances>)
            (sequences_of_previous_step: seq<Sequence_appearances>)
            =
            if Seq.isEmpty sequences_of_previous_step then
                all_sequences
            else
                let all_sequences =
                    all_sequences
                    |>Seq.append sequences_of_previous_step
                all_sequences
                |>Finding_many_repetitions.many_repetitions
                |>steps_of_finding_repetitions all_sequences
        ()        
        // steps_of_finding_repetitions
        //     []
        //     known_sequences


    type Benchmarking_finding_repetitions_with_concepts() =
        
        
        member _.number_concept = 
            built.Stencil.simple [
                "N","out";
                ",1","out";
                "out",",2";
                "out",";";
            ]

        member _.history_as_array =
            "N0,1,2,3,4,5,6,7,8,9;"
    //mom:   0123456789¹123456789²
            |>Seq.map string
            |>Array.ofSeq

        member _.history_as_figure =
            "N0,1,2,3,4,5,6,7,8,9;"
    //mom:   0123456789¹123456789²
            |>built.Figure.sequence_from_text
        
        [<Benchmark>]
        member this.transforming_sequence_into_a_figure()=
            this.history_as_array
            |>built.Figure.from_sequence

        [<Benchmark>]
        member this.applying_stencils_onto_sequence()=
            ()

        [<Benchmark>]
        member this.applying_stencils_onto_figure()=
            let result = 
                this.number_concept
                |>Applying_stencil.results_of_stencil_application this.history_as_figure
            
            (new Consumer()).Consume result

    [<Fact(Skip="slow")>] //
    let ``benchmark finding_repetitions_with_concepts``()=

        let config = 
            DefaultConfig.Instance.
                WithOptions(ConfigOptions.DisableOptimizationsValidator)

        BenchmarkRunner.Run<Benchmarking_finding_repetitions_with_concepts>(config) |> ignore