namespace rvinowise.ai

open FsUnit
open Xunit
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running
open rvinowise.extensions.benchmark
open rvinowise.ai
open rvinowise

type Benchmarking_modifications_of_history() =
    
    

    [<Benchmark>]
    member this.shifting_history_in_time() =
        ()
        
    

    [<Fact>]
    member _.run()=
        BenchmarkRunner.Run<Benchmarking_modifications_of_history>() 
