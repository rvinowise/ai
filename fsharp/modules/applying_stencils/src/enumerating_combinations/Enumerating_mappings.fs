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

        

        let rec occupy_next_free_target 
            (occupied_targets: Set<'Target>) 
            (possible_targets: 'Target list)
            =
            match possible_targets|>List.tryHead with
            |Some this_target ->
                match occupied_targets|>Set.contains this_target with
                |false -> 
                    possible_targets, 
                    occupied_targets
                    |>Set.add this_target
                |true -> occupy_next_free_target occupied_targets (possible_targets|>List.tail) 
            | None -> [], occupied_targets

        let first_combination ()
            //(elements_to_targets: Map<'Element, list<'Target>>)
            =
            let rec map_next_element 
                (occupied_targets: Set<'Target>) 
                (elements_to_targets_left: ('Element*'Target list) list)
                (mapped_elements: list<'Element*list<'Target>>)
                =
                match elements_to_targets_left with
                |[]->occupied_targets, mapped_elements
                |(element, possible_targets)::rest_elements_to_map ->
                    let targets,occupied_targets =
                        occupy_next_free_target
                            occupied_targets
                            possible_targets

                    match targets with
                    |[] -> occupied_targets, []
                    |targets ->
                        map_next_element
                            occupied_targets
                            rest_elements_to_map
                            ((element, targets)::mapped_elements )

            map_next_element
                Set.empty
                (
                    elements_to_targets
                    |>Seq.map (fun pair->pair.Key, pair.Value)
                    |>List.ofSeq
                )
                []
        
        (* the head of the Value is the currently mapped target, and the tail is the next possible targets *)
        let mutable current_combination: list<'Element*list<'Target>> = []
            

        let next_combination 
            (occupied_targets: Set<'Target>) 
            (base_combination:list<'Element* list<'Target>>)
            =
            let rec shift_orders_forward 
                (occupied_targets: Set<'Target>) 
                (next_orders: list<'Element* list<'Target>>)
                (reset_orders: list<'Element* list<'Target>>)
                =
                match next_orders with
                |[] ->occupied_targets, []
                |this_order::orders_left ->

                    let element = fst this_order
                    let current_occupied_target = 
                        this_order
                        |>snd
                        |>List.head
                    let next_possible_targets = 
                        this_order
                        |>snd
                        |>List.tail
                    let occupied_targets_wihout_current =
                        (occupied_targets|>Set.remove current_occupied_target)
                    let updated_targets, occupied_targets = 
                        occupy_next_free_target
                            occupied_targets_wihout_current
                            next_possible_targets
                    match updated_targets with
                    |[]->
                        let all_possible_targets = elements_to_targets[element]
                        shift_orders_forward
                            occupied_targets_wihout_current
                            orders_left
                            ((element, all_possible_targets)::reset_orders)

                    |found_targets -> 
                        occupied_targets,
                        (reset_orders|>List.rev) @ (element, found_targets)::orders_left

            shift_orders_forward
                occupied_targets
                base_combination
                []

        let simplified_combination (combination: list<'Element*list<'Target>>)
            =
            combination
            |>Seq.map (fun (element, targets) ->
                element,
                targets|>Seq.head
            )
            
        let mutable occupied_targets = Set.empty

        interface IEnumerator<seq<'Element*'Target>> with
            member this.Dispose() =()
        
            member this.Current: seq<'Element*'Target> =
                simplified_combination current_combination

        interface IEnumerator with
            member this.MoveNext():bool = 
                let (new_occupied_targets, new_current_combination) =
                    match current_combination with
                    |[]->
                        first_combination()
                    |_->
                        next_combination occupied_targets current_combination
                occupied_targets <- new_occupied_targets
                current_combination <- new_current_combination
                if current_combination=[] then
                    false
                else
                    true
            
            member this.Reset() =()
            
            member this.Current: obj = box (
                (this:>IEnumerator<seq<'Element*'Target>>).Current
            )


    type Generator_of_mappings<'Mapped, 'Target> when 
        'Mapped: comparison and
        'Target: comparison
        (targets: Map<'Mapped, list<'Target>> ) 
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
                Generator_of_mappings<Vertex_id, Vertex_id> ([
                    Vertex_id "a1", ["a6";"a7";"a8"]|>List.map Vertex_id 
                    Vertex_id "a2", ["a7";"a8"]|>List.map Vertex_id 
                ]|>Map.ofSeq)
            
            generator
            |>Seq.iter ignore
            
            generator
            |>should equal ([
                ["a1","a6"; "a2","a7"];
                ["a1","a7"; "a2","a8"];
                ["a1","a8"; "a2","a7"];
            ]|>Seq.map (Seq.map (fun (mapped, target) -> Vertex_id mapped, Vertex_id target)))

        