module rvinowise.ai.Finding_context_of_patterns

    open Xunit
    open FsUnit

    type Relative_interval = 
        struct
            val start: int
            val finish: int
        end

    type Context =
    |Before of Figure_id array
    |Inside of Relative_interval*Figure_id array
    |After of Figure_id array

    let context_of_sequence_appearance
        (history: Figure_id array)
        (mapped_sequence: (Figure_id*Moment) array)
        =
        let mapped_moments =
            mapped_sequence
            |>Array.map snd
        
        mapped_moments
        |>Array.fold (
            fun 
                (contexts, (last_index:int))
                moment
                ->
            let current_index = last_index+1
            let current_relative_interval =
                (last_index,current_index)
            let current_absolute_interval = 
                mapped_moments[last_index],mapped_moments[current_index]
            let current_context = 
                history[mapped_moments[last_index]..mapped_moments[current_index]]
            
            (current_relative_interval,current_context)
            ::contexts
            ,
            moment

        ) ([],-1)
        |>fst

        let last_sequence_moment = 
            mapped_moments
            |>Array.last
        mapped_moments
        |>Ara

    let history_slice
        (history: Figure_id array)
        (interval: Interval)
        =
        [|for i in interval.start..interval.finish -> i|]
        |>Array.zip
            history[interval.start..interval.finish]
            


    let sequence_mappings_onto_history
        (sequence: string)
        intervals
        history
        =
        let base_sequence =
            sequence
            |>Seq.map (string>>Figure_id)|>Array.ofSeq

        let sequence_appearances= 
            base_sequence
            ,
            intervals
            |>List.map Interval.ofPair

        let history_slices =
            sequence_appearances
            |>snd
            |>Seq.map (
                history_slice (
                    history
                )
            )
        history_slices
        |>Seq.map List.ofArray
        |>Seq.map (
            Mapping_sequence.map_sequence_onto_target (List.ofArray base_sequence)
        )|>Seq.choose id

    [<Fact>]
    let ``try context_of_sequence_appearance``()=
        let history =
            "1+1=2;ok;1+1=3;no;2+2=4;ok;"
    //mom:   012345   6789¹1   234567   89²  123456789
            |>built_from_text.Event_batches.signals_from_text
                (built_from_text.Event_batches.mood_changes_as_words_and_numbers "no" "ok")
            |>List.map fst
            |>Array.ofList

        let mapped_sequences =
            sequence_mappings_onto_history
                "+=;"
                [1,5; 13,17]
                history

        mapped_sequences
        |>Seq.map (
            context_of_sequence_appearance
                history
        )|>should equal (
            [
                ["1",0; "1",1; "2",2; "1+1=3;2+2=4;",3];
                ["1+1=2;1+1=3;2",0; "2",1; "4",2]
            ]|>List.map (List.map (fun (sequence,moment)->
                sequence
                |>Seq.map (string>>Figure_id)|>Array.ofSeq
                ,
                moment
            ))
        )