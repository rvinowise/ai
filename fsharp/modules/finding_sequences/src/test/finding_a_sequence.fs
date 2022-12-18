namespace rvinowise.ai.test

open System.Runtime.InteropServices
open Xunit
open FsUnit
open Xunit.Abstractions

module ``finding sequences``=
    open rvinowise.ai.Finding_sequences
    open rvinowise.ai


    type ``invoking native methods``(output: ITestOutputHelper)=
        let output = output

        [<Fact>]
        member this.``passing array of structures to a native method``()=
            repeated_pairs
                [|
                    Interval(0,1);
                    Interval(2,3);
                    Interval(4,5);
                |]
                [|
                    Interval(0,1);
                    Interval(2,3);
                    Interval(4,5);
                |]
            |> should equal
                [|
                    Interval(0,3);
                    Interval(2,5);
                |]
            
            ()

        [<Fact>]
        member this.``finding repeated pairs in big sequences``()=
            let items_amount = 100000
            
            let heads = [|
                for i in 0..items_amount ->
                    Interval(i,i+1)
            |]
            let tails = [|
                for i in 0..items_amount ->
                    Interval(i,i+1)
            |]
            
            repeated_pairs heads tails
            |>should equal 
                [|
                    for i in 0..items_amount-2 ->
                        Interval(i,i+3)
                |]