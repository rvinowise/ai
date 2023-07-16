namespace rvinowise.ai

module Mapping_sequence = 

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
        rest_mappee
        rest_target
        mapping
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
                    tail
                    rest_target
                    (mapped_signal::mapping)
            |None->None
            


    let map_sequence_onto_target
        mappee
        target
        =
        map_next_signal
            mappee
            target
            []
        |>Option.map List.rev