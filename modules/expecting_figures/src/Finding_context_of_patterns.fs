module rvinowise.ai.Finding_context_of_patterns

    open Xunit
    open FsUnit


    let context_of_sequence_appearance
        (history: Figure_id array)
        (mapped_sequence: (Figure_id*Moment) array)
        =
        let mapped_moments =
            mapped_sequence
            |>Array.map snd
        
        let mapped_moments_without_last = mapped_moments[0..mapped_moments.Length-2]
        let context_inside = 
            mapped_moments_without_last
            |>Array.fold (
                fun 
                    (found_contexts, index)
                    moment
                    ->
                let next_index = index+1
                let current_relative_interval =
                    (index,next_index)
                let current_context = 
                    history[moment+1..mapped_moments[next_index]-1]
                
                (current_relative_interval,current_context)
                ::found_contexts
                ,
                next_index

            ) ([],0)
            |>fst
            |>List.rev

        let context_before =
            history[.. (Array.head mapped_moments)-1]
        let context_after =
            history[(Array.last mapped_moments)+1 ..]
        
        context_before,context_inside,context_after

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
            |>History_from_text.signals_from_text
                (History_from_text.mood_changes_as_words_and_numbers "no" "ok")
            |>List.map fst
            |>Array.ofList

        let mapped_sequences =
            sequence_mappings_onto_history
                "+=;"
                [1,5; 13,17]
                history
            |>Seq.map Array.ofList

        mapped_sequences
        |>Seq.map (
            context_of_sequence_appearance
                history
        )|>should equal (
            [
                "1",[0,1,"1"; 1,2,"2"],"1+1=3;2+2=4;";
                "1+1=2;1+1=3;2", [0,1,"2"; 1,2,"4"],""
            ]|>List.map (fun (before,inside,after)->
                before
                |>Seq.map (string>>Figure_id)|>Array.ofSeq
                ,
                inside
                |>List.map (fun (start,finish,sequence)->
                    (start,finish)
                    ,
                    sequence
                    |>Seq.map (string>>Figure_id)|>Array.ofSeq
                )
                ,
                after
                |>Seq.map (string>>Figure_id)|>Array.ofSeq
            )
        )