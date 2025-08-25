namespace rvinowise.ai.test

open BenchmarkDotNet.Engines
open Xunit

open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

open rvinowise
open rvinowise.ai
open rvinowise.ai.mapping_graph_impl
open rvinowise.extensions.benchmark


module prolongation_of_mapping =

    type Mappee_case = {
        simple: Figure<Mapping_function_id>
        conditional: Conditional_figure<Mapping_function_id>
    }
    
    let empty_figure_for_mapping =
        built.Figure.from_tuples
            Mapping_functions_registry.onto_exact_figure
            Mapping_functions_registry.id_into_name
            []
    
    type Benchmarking_prolongation_of_mapping() =

        member val mappees = [
            {
                Parameter.value=
                    {
                        simple=
                            example.Stencil.a_long_stencil.figure.existing;
                        conditional=
                            example.Stencil.a_long_stencil.figure
                    }
                
                name="a_long_mappee"
            };
            {
                Parameter.value=
                    {
                        simple=
                            example.Stencil.a_fitting_stencil.figure.existing;
                        conditional=
                            example.Stencil.a_fitting_stencil.figure
                    }
                name="a_simple_mappee"
            };
            {
                Parameter.value=
                    {
                        simple=
                            example.Stencil.a_stencil_with_huge_beginning.figure.existing;
                        conditional=
                            example.Stencil.a_stencil_with_huge_beginning.figure
                    }
                name="a_mappee_with_huge_beginning"
            }
        ]
        
        member val target_figures = [
            {Parameter.value= example.Figure.a_long_figure; 
            name="a_long_figure"};
            {Parameter.value= example.Figure.a_high_level_relatively_simple_figure; 
            name="a_high_level_relatively_simple_figure"};
            {value= example.Figure.a_figure_with_huge_beginning;
            name="a_figure_with_huge_beginning"}
        ]
        
        
        
        [<ParamsSource("mappees")>]
        member val mappee =
            
            {
                value={
                    simple=empty_figure_for_mapping
                    conditional= built.Conditional_figure.from_figure_without_impossibles empty_figure_for_mapping 
                }
                name="default"
            } with get, set

        [<ParamsSource("target_figures")>]
        member val target_figure = {value=example.Figure.empty; name="default"} with get, set

        
  
        [<Benchmark>]
        (*not updated (old incomplete algorithm)*)
        member this.prolongation_of_mutable_mapping()=
            Mapping_graph_with_mutable_mapping.map_figure_onto_target
                Mapping_functions_registry.id_into_function
                this.target_figure.value
                this.mappee.value.simple
            |> Consumer().Consume
        
        [<Benchmark>]
        member this.prolongation_of_immutable_mapping()=
            Mapping_graph_with_immutable_mapping.map_figure_onto_target
                Mapping_functions_registry.id_into_function
                this.target_figure.value
                this.mappee.value.simple
            |> Consumer().Consume
        
        [<Benchmark>]
        (*without any actual conditions, simply to compare it with unconditional simple mappings *)
        member this.prolongation_of_immutable_conditional_mapping()= 
            this.mappee.value.conditional
            |>Mapping_graph_with_immutable_mapping.map_conditional_figure_onto_target
                Mapping_functions_registry.id_into_function
                Map.empty
                this.target_figure.value
            |> Consumer().Consume
        
    [<Fact(Skip="slow")>] //
    let run_benchmark()=

        let config = 
            DefaultConfig.Instance.
                WithOptions(ConfigOptions.DisableOptimizationsValidator)

        BenchmarkRunner.Run<Benchmarking_prolongation_of_mapping>(config) |> ignore

