namespace rvinowise.ai.test

open Xunit
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running


module Finding_repetitions_repetitive_benchmark =
    open rvinowise.ai

    type Finding_repetitions_repetitive_benchmark() =
        
        [<Params(5, 50(*, 10000*))>]
        member val items_amount = 0 with get, set
        
        member this.long_sequence_of_input() = 
            [|
                for i in 0..this.items_amount -> 
                    Interval.from_int i (i+1)
            |]

        member this.heads = this.long_sequence_of_input()

        [<Benchmark>]
        member this.prepare_long_sequences_of_input()=
            this.long_sequence_of_input()

        [<Benchmark>]
        member this.repeated_pair_in_big_sequences()=
            Finding_repetitions.repeated_pair 
                (Finding_repetitions.halves_are_close_enough 1)
                this.heads this.heads
            

    [<Fact(Skip="slow")>] //
    let benchmark()=
        let config = 
            DefaultConfig.Instance.
                WithOptions(ConfigOptions.DisableOptimizationsValidator)

        BenchmarkRunner.Run<Finding_repetitions_repetitive_benchmark>(config) |> ignore



