namespace rvinowise.ai

    open Xunit
    open FsUnit

    open System.Collections.Generic
    

    type Generator_of_mappings<'Mapped, 'Target> (
        targets: Map<'Mapped, seq<'Target>> 
    ) =
        interface IEnumerable< seq<'Mapped*'Target> > with
            member this.GetEnumerator () =
                new Generator_of_mappings_enumerator(targets)
        
        interface IEnumerable with
            member this.GetEnumerator () =
                (this:> IEnumerable<seq<'Mapped*'Target>> ).GetEnumerator():>IEnumerator

    
    type Generator_of_mappings_enumerator (targets: Map<'Mapped, seq<'Target>>) =
        
        interface IEnumerator<seq<'Mapped*'Target>> with
            
            member this.Dispose() =()
            member this.Current():seq<'Mapped*'Target> =()

        interface IEnumerator with
            member this.MoveNext():bool =()
            member this.Reset() =()
            member this.Current():obj = box (this.Current())

    module Enumerating_mappings =


        [<Fact>]
        let ``enumerate over mappings``()=
            let generator = 
                new Generator_of_mappings ([
                    "a1", ["a6";"a7";"a8"]
                    "a2", ["a7";"a8"]
                ]|>Map.ofSeq)
            
            generator
            |>should equal [
                ["a1","a6"; "a2","a7"];
                ["a1","a7"; "a2","a8"];
                ["a1","a8"; "a2","a7"];
            ]