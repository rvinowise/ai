namespace rvinowise.ai.test

open BenchmarkDotNet.Engines
open Xunit

open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

open rvinowise.ai
open rvinowise.ai.applying_stencil_impl
open rvinowise.extensions.benchmark


module mapping_first_nodes =

    type Benchmarking_mapping_first_nodes() =

        member val stencils = [
            {Parameter.value= example.Stencil.a_fitting_stencil; 
            name="a_fitting_stencil"};
            {value= example.Stencil.a_stencil_with_huge_beginning; 
            name="a_stencil_with_huge_beginning"}
        ]
        member val target_figures = [
            {Parameter.value= example.Figure.a_high_level_relatively_simple_figure; 
            name="a_high_level_relatively_simple_figure"};
            {value= example.Figure.a_figure_with_huge_beginning;
            name="a_figure_with_huge_beginning"}
        ]

        [<ParamsSource("stencils")>]
        member val stencil = {value=example.Stencil.empty; name="default"} with get, set

        [<ParamsSource("target_figures")>]
        member val target_figure = {value=built.Figure.empty; name="default"} with get, set

        [<Benchmark>]
        member this.breaking_recursion()=
            Map_first_nodes.``map_first_nodes(breaking recursion)`` 
                this.stencil.value
                this.target_figure.value
            |> Consumer().Consume
  
        [<Benchmark>]
        member this.checking_after_full_calculation()=
            Map_first_nodes.``map_first_nodes(checking after full calculation)``
                this.stencil.value
                this.target_figure.value
            |> Consumer().Consume
        
        
    [<Fact(Skip="slow")>] //
    let run_benchmark()=

        let config = 
            DefaultConfig.Instance.
                WithOptions(ConfigOptions.DisableOptimizationsValidator)

        BenchmarkRunner.Run<Benchmarking_mapping_first_nodes>(config) |> ignore


    [<Fact>]
    let ``profile map_first_nodes(breaking recursion) big_stencil``()=
        Map_first_nodes.``map_first_nodes(breaking recursion)`` 
            example.Stencil.a_stencil_with_huge_beginning
            example.Figure.a_high_level_relatively_simple_figure
        |> Consumer().Consume

    [<Fact>]
    let ``profile map_first_nodes(breaking recursion) small_stencil``()=
        Map_first_nodes.``map_first_nodes(breaking recursion)`` 
            example.Stencil.a_fitting_stencil
            example.Figure.a_high_level_relatively_simple_figure
        |> Consumer().Consume
    
    [<Fact>]
    let ``profile map_first_nodes(simple) big_stencil``()=
        Map_first_nodes.``map_first_nodes(checking after full calculation)`` 
            example.Stencil.a_stencil_with_huge_beginning
            example.Figure.a_high_level_relatively_simple_figure
        |> Consumer().Consume