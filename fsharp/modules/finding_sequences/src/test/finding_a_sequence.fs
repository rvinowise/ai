namespace rvinowise.ai.test

open System.Runtime.InteropServices
open Xunit
open Xunit.Abstractions

module ``finding sequences``=
    open rvinowise.ai.Finding_sequences
    open rvinowise.ai


    type ``invoking native methods``(output: ITestOutputHelper)=
        let output = output

        [<Fact>]
        member this.``passing array of structures to a native method``()=
            let mutable found_repetitions: Interval = Interval.moment(0UL)
            let count = find_repeated_pairs(
                [|
                    Interval(0,1);
                    Interval(2,3);
                    Interval(4,5);
                |],3,
                [|
                    Interval(0,1);
                    Interval(2,3);
                    Interval(4,5);
                |],3,
                &found_repetitions
            )
            output.WriteLine($"result = {found_repetitions}")
            
            let result = Array.create count (Interval.moment 0)
            Marshal.Copy(found_repetitions, result, 0, count);
            
            ()