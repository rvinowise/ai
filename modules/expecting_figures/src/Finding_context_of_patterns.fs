module rvinowise.ai.Finding_context_of_patterns

    open Xunit
    open FsUnit

    let context_of_sequence_appearance
        history
        (mapped_sequence )
        =()


    let history_slice
        (history: Figure_id array)
        (interval: Interval)
        =
        history[interval.start..interval.finish]
        |>Array.zip
            [|for i in interval.start..interval.finish -> i|]


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
                    |>Array.map fst
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
        )|>should equal [
            
        ]