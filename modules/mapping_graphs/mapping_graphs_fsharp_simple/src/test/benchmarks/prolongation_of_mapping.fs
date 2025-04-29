namespace rvinowise.ai.test

open BenchmarkDotNet.Engines
open Xunit

open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

open rvinowise.ai
open rvinowise.ai.mapping_graph_impl
open rvinowise.extensions.benchmark


module prolongation_of_mapping =

    
    type Benchmarking_prolongation_of_mapping() =

        member val mappees = [
            {Parameter.value= example.Stencil.a_long_stencil|>Figure_from_stencil.convert; 
            name="a_long_stencil"};
            {Parameter.value= example.Stencil.a_fitting_stencil|>Figure_from_stencil.convert; 
            name="a_fitting_stencil"};
            {value= example.Stencil.a_stencil_with_huge_beginning|>Figure_from_stencil.convert; 
            name="a_stencil_with_huge_beginning"}
        ]
        member val target_figures = [
            {Parameter.value= example.Figure.a_long_figure; 
            name="a_long_figure"};
            {Parameter.value= example.Figure.a_high_level_relatively_simple_figure; 
            name="a_high_level_relatively_simple_figure"};
            {value= example.Figure.a_figure_with_huge_beginning;
            name="a_figure_with_huge_beginning"}
        ]
        // member val prolongating_function = [
        //     {Parameter.value= Mapping_graph_with_mutable_mapping.map_figure_onto_target; 
        //     name="using a mutable dictionary"};
        //     {Parameter.value= Mapping_graph_with_immutable_mapping.map_figure_onto_target; 
        //     name="using a immutable dictionary"};
        // ]

        [<ParamsSource("mappees")>]
        member val mappee = {value=example.Figure.empty; name="default"} with get, set

        [<ParamsSource("target_figures")>]
        member val target_figure = {value=example.Figure.empty; name="default"} with get, set

        
  
        [<Benchmark>]
        member this.prolongation_of_mutable_mapping()=
            Mapping_graph_with_mutable_mapping.map_figure_onto_target
                this.target_figure.value
                this.mappee.value
            |> Consumer().Consume
        
        [<Benchmark>]
        member this.prolongation_of_immutable_mapping()=
            Mapping_graph_with_immutable_mapping.map_figure_onto_target
                this.target_figure.value
                this.mappee.value
            |> Consumer().Consume
        
        
    [<Fact(Skip="slow")>] //
    let run_benchmark()=

        let config = 
            DefaultConfig.Instance.
                WithOptions(ConfigOptions.DisableOptimizationsValidator)

        BenchmarkRunner.Run<Benchmarking_prolongation_of_mapping>(config) |> ignore

