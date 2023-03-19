namespace rvinowise.ai.test

open Xunit
open FsUnit

open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

open rvinowise.ai
open rvinowise.ai.ui
open rvinowise.ai.mapping_stencils



module enumerating_combinations_benchmark =

    type Benchmarking_enumerating_combinations() =

        [<Benchmark>]
        member this.generator_of_mappings_with_indices()=
            Applying_stencil.map_first_nodes 
                example.Stencil.a_fitting_stencil
                example.Figure.a_high_level_relatively_simple_figure
            
        [<Benchmark>]
        member this.generator_of_mappings_with_objects()=
            


    [<Fact>] //(Skip="slow")
    let benchmark()=

        let config = 
            DefaultConfig.Instance.
                WithOptions(ConfigOptions.DisableOptimizationsValidator)

        BenchmarkRunner.Run<Benchmarking_enumerating_combinations>(config) |> ignore