namespace rvinowise.ai.test

open BenchmarkDotNet.Engines
open Xunit

open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

open rvinowise.ai
open rvinowise.ai.mapping_graph_impl
open rvinowise.extensions.benchmark


module mapping_first_nodes =

    type Benchmarking_mapping_first_nodes() =

        member val mappees = [
            {Parameter.value= example.Figure.fitting_stencil_as_figure; 
            name="a_fitting_stencil"};
            {value= example.Stencil.a_stencil_with_huge_beginning|>Figure_from_stencil.convert; 
            name="a_stencil_with_huge_beginning"}
        ]
        member val target_figures = [
            {Parameter.value= example.Figure.a_high_level_relatively_simple_figure; 
            name="a_high_level_relatively_simple_figure"};
            {value= example.Figure.a_figure_with_huge_beginning;
            name="a_figure_with_huge_beginning"}
        ]

        [<ParamsSource("mappees")>]
        member val mappee = {value=example.Figure.empty; name="default"} with get, set

        [<ParamsSource("target_figures")>]
        member val target_figure = {value=example.Figure.empty; name="default"} with get, set

        
  
        [<Benchmark>]
        member this.checking_after_full_calculation()=
            Map_first_nodes.map_first_nodes
                this.mappee.value
                this.target_figure.value
            |> Consumer().Consume
        
        
    [<Fact(Skip="slow")>] //
    let run_benchmark()=

        let config = 
            DefaultConfig.Instance.
                WithOptions(ConfigOptions.DisableOptimizationsValidator)

        BenchmarkRunner.Run<Benchmarking_mapping_first_nodes>(config) |> ignore

