namespace rvinowise.ai.test

open BenchmarkDotNet.Engines
open Xunit

open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

open rvinowise.ai
open rvinowise.ai.applying_stencil_impl



module mapping_first_nodes =

    type Benchmarking_mapping_first_nodes() =

        member val stencils = [
            example.Stencil.a_fitting_stencil;
            example.Stencil.a_stencil_with_huge_beginning
        ]
        member val target_figures = [
            example.Figure.a_high_level_relatively_simple_figure;
            example.Figure.a_figure_with_huge_beginning;
        ]

        [<ParamsSource("stencils")>]
        member val stencil = built.Stencil.simple_without_separator [] with get, set

        [<ParamsSource("target_figures")>]
        member val target_figure = built.Figure.empty with get, set

        [<Benchmark>]
        member this.breaking_recursion()=
            Map_first_nodes.``map_first_nodes(breaking recursion)`` 
                this.stencil
                example.Figure.a_figure_with_huge_beginning
            |> Consumer().Consume
  
        [<Benchmark>]
        member this.checking_after_full_calculation()=
            Map_first_nodes.``map_first_nodes(checking after full calculation)``
                this.stencil
                example.Figure.a_figure_with_huge_beginning
            |> Consumer().Consume
        
        
    [<Fact>] //(Skip="slow")
    let run_benchmark()=

        let config = 
            DefaultConfig.Instance.
                WithOptions(ConfigOptions.DisableOptimizationsValidator)

        BenchmarkRunner.Run<Benchmarking_mapping_first_nodes>(config) |> ignore

