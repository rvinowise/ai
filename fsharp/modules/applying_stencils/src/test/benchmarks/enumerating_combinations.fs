namespace rvinowise.ai.test

open BenchmarkDotNet.Engines
open Xunit

open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

open rvinowise.ai



module enumerating_combinations =

    type Benchmarking_enumerating_combinations() =

        [<Benchmark>]
        member this.generator_of_mappings_with_indices()=
            Applying_stencil.map_first_nodes 
                example.Stencil.a_fitting_stencil
                example.Figure.a_high_level_relatively_simple_figure
            |> Consumer().Consume
  

    [<Fact(Skip="slow")>] //
    let run_benchmark()=

        let config = 
            DefaultConfig.Instance.
                WithOptions(ConfigOptions.DisableOptimizationsValidator)

        BenchmarkRunner.Run<Benchmarking_enumerating_combinations>(config) |> ignore

