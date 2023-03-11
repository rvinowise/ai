namespace rvinowise.ai

    open Xunit
    open FsUnit

    open System.Collections.Generic
    open System.Collections
    


    type Generator_of_mappings_enumerator<'Mapped, 'Target> when 
        'Mapped: comparison and 
        'Target: equality and 'Target: comparison
        (available_targets: Map<'Mapped, array<'Target>>) 
        =
        // let occupied_targets = 
        //     available_targets
        //     |>Seq.collect (fun pair->
        //         pair.Value
        //     )
        //     |>Seq.distinct
        //     |>Seq.map (fun target -> (target,false))
        //     |>Map.ofSeq

        let mutable occupied_targets = Set.empty

        let first_free_target 
            (possible_targets: 'Target list)
            =
            let try_next_target 
                (possible_targets: 'Target list) 
                =
                match possible_targets with
                |this_target::left_targets ->
                    match occupied_targets|>Set.exists target with
                    |false -> this_target
                    |true -> try_next_target left_targets
                | [] -> 
                
            try_next_target possible_targets
                


        let mutable current_combination:list<'Mapped*'Target> = 
            available_targets
            |>Seq.map (fun pair->
                let mapped = pair.Key
                let possible_targets = pair.Value
                (
                    mapped,
                    (first_free_target possible_targets)
                )
            )
            

        interface IEnumerator<seq<'Mapped*'Target>> with
            member this.Dispose() =()
        
            member this.Current: seq<'Mapped*'Target> =
                current_combination

        interface IEnumerator with
            member this.MoveNext():bool = 
                current_combination
            
            member this.Reset() =()
            
            member this.Current: obj = box (
                (this:>IEnumerator<seq<'Mapped*'Target>>).Current
            )


    type Generator_of_mappings<'Mapped, 'Target> when 
        'Mapped: comparison and
        'Target: comparison
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

        