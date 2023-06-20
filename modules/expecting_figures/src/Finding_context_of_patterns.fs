module rvinowise.ai.Finding_context_of_patterns

    open Xunit

    let context_of_sequence_appearance
        (sequence_appearance: )




    [<Fact>]
    let ``try context_of_sequence_appearance``()=
        let history =
            "1+1=2;ok;1+1=3;no;2+2=4;ok;"
    //mom:   012345   6789¹1   234567   89²  123456789
            |>built_from_text.Event_batches.event_batches_from_text
                (built_from_text.Event_batches.mood_changes_as_words_and_numbers "no" "ok")
            
        let good_sequences = 
            Finding_repeatedly_good_sequences.find_good_sequences_in_batches 
                history

        context_of_sequence_appearance