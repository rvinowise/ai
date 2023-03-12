namespace rvinowise.ai

    open Xunit
    open FsUnit

    open System.Collections.Generic
    open System.Collections
    


    type Generator_of_mappings_enumerator<'Element, 'Target> when 
        'Element: comparison and 
        'Target: equality and 'Target: comparison
        (elements_to_targets: Map<'Element, list<'Target>>) 
        =

        let mutable occupied_targets = Set.empty

        
        let rec occupy_next_free_target 
            (occupied_targets: Set<'Target>) 
            (possible_targets: 'Target list)
            =
            match possible_targets with
            |this_target::left_targets ->
                match occupied_targets|>Set.contains this_target with
                |false -> 
                    Some this_target, 
                    occupied_targets|>Set.add this_target
                |true -> occupy_next_free_target occupied_targets left_targets 
            | [] -> None, occupied_targets

        
        let mutable current_combination:list<'Element*'Target> = 
            
            let rec map_next_element 
                (occupied_targets: Set<'Target>) 
                (elements_to_targets_left: ('Element*'Target list) list)
                (mapped_elements: ('Element*'Target) list)
                =
                match elements_to_targets_left with
                |[]->mapped_elements
                |(element, possible_targets)::rest_elements_to_map ->
                    let target,occupied_targets =
                        occupy_next_free_target
                            occupied_targets
                            possible_targets

                    match target with
                    |None -> []
                    |Some target ->
                        map_next_element
                            occupied_targets
                            rest_elements_to_map
                            ((element,target)::mapped_elements)

            map_next_element
                Set.empty
                (
                    elements_to_targets
                    |>Seq.map (fun pair->pair.Key, pair.Value)
                    |>List.ofSeq
                )
                []
            

        interface IEnumerator<seq<'Element*'Target>> with
            member this.Dispose() =()
        
            member this.Current: seq<'Element*'Target> =
                current_combination

        interface IEnumerator with
            member this.MoveNext():bool = 
                current_combination
            
            member this.Reset() =()
            
            member this.Current: obj = box (
                (this:>IEnumerator<seq<'Element*'Target>>).Current
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

        