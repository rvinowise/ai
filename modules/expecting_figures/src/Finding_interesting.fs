module rvinowise.ai.Finding_interesting
    
    open FsUnit
    open Xunit
    open rvinowise


    let sequences_which_appeared_in_interval
        (appearances_of_sequences: (Sequence*Interval array) seq)
        (interval: Interval)
        =
        appearances_of_sequences
        |>Seq.map (fun (sequence, appearances) ->
            sequence
            ,
            appearances
            |>Seq.filter(fun appearance ->
                interval.start <= appearance.start
                ||
                interval.finish >= appearance.finish
            )
        )
        |>Seq.filter (snd>>Seq.isEmpty>>not)


    let find_good_sequences
        (sequence_history: (Sequence*Interval array) seq)
        (mood_changes: (Moment*Mood) seq)
        =
        mood_changes
        |>Seq.filter (snd>>Mood.is_good)
        |>Mood_history.intervals_changing_mood
        |>Seq.map fst
        |>Seq.map (sequences_which_appeared_in_interval sequence_history)
        |>Seq.allPairs
        |>Seq.filter 


    [<Fact>]
    let ``try find_good_sequences``()=
        let history =
            "1+1=2;ok;1+1=3;no;2+2=4;ok;"
    //mom:   0123456  789¹123  456789²  123456789
            |>built_from_text.Event_batches.event_batches_from_text
        let signal_history =
            history
            |>Event_batches.only_signals
            |>Event_batches.to_sequence_appearances
        let mood_history = 
            history
            |>Event_batches.only_mood_changes
            
        find_good_sequences 
            signal_history
            mood_history
        |>Appearances.sequence_appearances_to_text_and_tuples
        |>Set.ofSeq
        |>Set.isProperSubset (
            [
                "+=;",
                [1,5;15,19]
            ]|>Set.ofList
        )|>should equal true