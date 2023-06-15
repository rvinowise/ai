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
            |>Array.filter(fun appearance ->
                interval.start <= appearance.start
                ||
                interval.finish >= appearance.finish
            )
        )
        |>Seq.filter (snd>>Seq.isEmpty>>not)



    let unite_appearances_of_same_sequences
        (histories: (Sequence*Interval array) seq)
        =
        Map.empty
        |>Seq.foldBack (fun (sequence, history) sequence_histories ->
            sequence_histories
            |>Map.add sequence (
                sequence_histories
                |>Map.tryFind sequence
                |>Option.defaultValue [||]
                |>Array.append history
            )

        ) histories

    let first_interval_is_before_second
        (interval1:Interval, interval2:Interval)
        =
        interval1.start < interval2.start
        &&
        interval1.finish <= interval2.finish
    
    let find_good_sequences
        (sequence_history: (Sequence*Interval array) seq)
        (mood_changes: (Moment*Mood) seq)
        =
        let all_intervals = 
            mood_changes
            |>Seq.filter (snd>>Mood.is_good)
            |>Mood_history.intervals_changing_mood
            |>Seq.map fst
        let interval_to_histories =
            all_intervals
            |>Seq.map (fun interval->
                interval
                ,
                sequences_which_appeared_in_interval sequence_history interval
            )
            |>Map.ofSeq

        let interval_pairs =
            (all_intervals,all_intervals)
            ||>Seq.allPairs
            |>Seq.filter first_interval_is_before_second
            

        interval_pairs
        |>Seq.collect (fun (interval1,interval2)->
            let sequence_appearances1 = interval_to_histories[interval1]
            let sequence_appearances2 = interval_to_histories[interval2]
            let in_interval1, in_interval2 = 
                Finding_many_repetitions.repetitions_in_2_intervals
                    Finding_repetitions.all_halves
                    sequence_appearances1
                    sequence_appearances2
            [in_interval1;in_interval2]
        )|>Seq.concat
        |>unite_appearances_of_same_sequences



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
        |>extensions.Map.toPairs
        |>Appearances.sequence_appearances_to_text_and_tuples
        |>Set.ofSeq
        |>Set.isProperSubset (
            [
                "+=;",
                [1,5;15,19]
            ]|>Set.ofList
        )|>should equal true