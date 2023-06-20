namespace rvinowise.ai

module Mapping_sequence = 
    open rvinowise

    open Xunit
    open FsUnit


    let rec find_next_signal_in_target
        (signal: Figure_id)
        (target: (Figure_id*Moment) list)
        =
        match target with
        |head::tail->
            if signal = (fst head) then
                Some (head,tail)
            else
                find_next_signal_in_target
                    signal
                    tail
        |[]->
            None

    let rec map_next_signal
        rest_target
        rest_mappee
        (mapping: (Figure_id*Moment) list)
        =
        match rest_mappee with
        |[]->Some mapping
        |head::tail->
            match
                find_next_signal_in_target 
                    head 
                    rest_target 
            with
            |Some (mapped_signal,rest_target) ->
                map_next_signal
                    rest_target
                    tail
                    (mapped_signal::mapping)
            |None->None
            


    let map_sequence_onto_target
        (target: Sequence)
        (mappee: Sequence)
        =
        map_next_signal
            target
            mappee
            []