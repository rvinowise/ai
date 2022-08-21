namespace rvinowise.ai.search

open System
open NUnit.Framework
open FsUnit

open rvinowise


module found_sequence =

    [<TestFixture>]
    type ``two signals repeat twice`` () =

        let appearances_a: ai.Interval list = [
            {head=1;tail=2};
            {head=3;tail=4};
            {head=5;tail=9};
        ]
        let appearances_b: ai.Interval list = [
            {head=3;tail=4};
            {head=3;tail=4};
            {head=10;tail=11};
        ]
        
        let expected_pairs: ai.Interval list = [
            {head=1;tail=4};
            {head=5;tail=11};
        ]
        
        [<Test>]
        member this.a_repeated_pair_is_found() =
            let found = ai.sequence.Found.repeated_pair_in_sequences appearances_a appearances_b
            Assert.AreEqual(found, expected_pairs)

    
    [<TestFixture>]
    type ``no repetition exists`` () =

        let appearances_a: ai.Interval list = [
            {head=1;tail=2};
            {head=3;tail=4};
            {head=5;tail=9};
        ]
        let appearances_b: ai.Interval list = [
            {head=6;tail=10};
            {head=10;tail=11};
            {head=12;tail=100};
        ]
      
        
        [<Test>]
        member this.``a repeated pair is found``() =
            let found = ai.sequence.Found.repeated_pair_in_sequences appearances_a appearances_b
            found |> should equal []