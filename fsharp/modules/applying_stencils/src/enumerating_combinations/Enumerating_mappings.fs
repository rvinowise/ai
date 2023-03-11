namespace rvinowise.ai

    open Xunit
    open FsUnit

    open System.Collections.Generic
    open System.Collections
    


    type Generator_of_mappings_enumerator<'Mapped, 'Target> when 'Mapped: comparison
        (targets: Map<'Mapped, seq<'Target>>) 
        =
        
        interface IEnumerator<seq<'Mapped*'Target>> with
            member this.Dispose() =()
            
            member this.Current: seq<'Mapped*'Target> =
                []

        interface IEnumerator with
            member this.MoveNext():bool = 
                false
            
            member this.Reset() =()
            
            member this.Current: obj = box (
                (this:>IEnumerator<seq<'Mapped*'Target>>).Current
            )


    type Generator_of_mappings<'Mapped, 'Target> when 'Mapped: comparison 
        (targets: Map<'Mapped, seq<'Target>> ) 
        =
        interface IEnumerable< seq<'Mapped*'Target> > with
            member this.GetEnumerator () =
                new Generator_of_mappings_enumerator<'Mapped, 'Target>(targets)
        
        interface IEnumerable with
            member this.GetEnumerator () =
                (this:> IEnumerable<seq<'Mapped*'Target>> ).GetEnumerator():>IEnumerator

    
    

    module Enumerating_mappings =

        open rvinowise.ai.mapping_stencils

        [<Fact>]
        let ``enumerate over mappings of one figure``()=
            let generator = 
                new Generator_of_mappings<Vertex_id, Vertex_id> ([
                    Vertex_id "a1", ["a6";"a7";"a8"]|>Seq.map Vertex_id 
                    Vertex_id "a2", ["a7";"a8"]|>Seq.map Vertex_id 
                ]|>Map.ofSeq)
            
            generator
            |>should equal ([
                ["a1","a6"; "a2","a7"];
                ["a1","a7"; "a2","a8"];
                ["a1","a8"; "a2","a7"];
            ]|>Seq.map (Seq.map (fun (mapped, target) -> Vertex_id mapped, Vertex_id target)))

        