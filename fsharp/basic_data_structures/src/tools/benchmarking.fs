namespace rvinowise.extensions

open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

[<AutoOpen>]
module Benchmarking=


    let benchmark<'Fixture>() =

        let config = 
            DefaultConfig.Instance.
                WithOptions(ConfigOptions.DisableOptimizationsValidator)

        BenchmarkRunner.Run<'Fixture>(config) |> ignore