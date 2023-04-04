namespace rvinowise.extensions

open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running

    module Benchmarking=


        let run_benchmark<'Fixture>() =

            let config = 
                DefaultConfig.Instance.
                    WithOptions(ConfigOptions.DisableOptimizationsValidator)

            BenchmarkRunner.Run<'Fixture>(config) |> ignore
    

namespace rvinowise.extensions.benchmark

    type Parameter<'T> = {
        value: 'T;
        name: string
    }
    with
        override this.ToString()=this.name