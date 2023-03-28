namespace rvinowise.ai.test

open BenchmarkDotNet.Engines
open Xunit

open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

open rvinowise.ai



module querying_stencil =

    type Benchmarking_querying_stencil() =

        member this.stencil = example.Stencil.a_fitting_stencil
        member this.figure = example.Figure.a_figure_with_huge_beginning

        [<Benchmark>]
        member this.map_first_nodes__double_query()=
            Applying_stencil.map_first_nodes this.stencil this.figure
            |>Consumer().Consume


    [<Fact>] //(Skip="slow")
    let run_benchmark()=
        let config = 
            DefaultConfig.Instance.
                WithOptions(ConfigOptions.DisableOptimizationsValidator)

        BenchmarkRunner.Run<Benchmarking_querying_stencil>(config) |> ignore


    [<Fact>]
    let profile()=
        let stencil = example.Stencil.a_fitting_stencil
        let figure = example.Figure.a_figure_with_huge_beginning
        
        Applying_stencil.map_first_nodes stencil figure
        |>Consumer().Consume
        
        
