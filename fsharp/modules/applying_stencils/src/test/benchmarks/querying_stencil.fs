namespace rvinowise.ai.test

open BenchmarkDotNet.Engines
open Xunit
open FsUnit

open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

open rvinowise.ai
open rvinowise.ai.ui
open rvinowise.ai.mapping_stencils



module querying_stencil =

    type Benchmarking_querying_stencil() =

        member this.stencil = example.Stencil.a_fitting_stencil
        member this.figure = example.Figure.a_high_level_relatively_simple_figure

        [<Benchmark>]
        member this.``query first nodes several times, but no unpacking tuples``()=
            
            let first_stencil_subfigures_with_figures =
                this.stencil.edges
                |>Edges.first_vertices
                |>Stencil.only_subfigures_with_figures this.stencil

            let first_subfigures_of_stencil = 
                first_stencil_subfigures_with_figures
                |>Seq.map fst

            let figures_to_map = 
                first_stencil_subfigures_with_figures
                |>Seq.map snd
                |>Seq.distinct

            first_subfigures_of_stencil|>Consumer().Consume
            figures_to_map|>Consumer().Consume
  
        [<Benchmark>]
        member this.``query first nodes once, but then unpack tuples``()=
            Applying_stencil.map_first_nodes 
                example.Stencil.a_fitting_stencil
                example.Figure.a_high_level_relatively_simple_figure
            |> Consumer().Consume

    [<Fact>] //(Skip="slow")
    let run_benchmark()=
        BenchmarkRunner.Run<Benchmarking_querying_stencil>() |> ignore

