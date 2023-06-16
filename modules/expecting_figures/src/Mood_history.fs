module rvinowise.ai.Mood_history
    open FsUnit
    open Xunit

    let mood_change_from_start_to_finish
        (changes: (Moment*Mood) array)
        (start_index:int)
        (finish_index:int)
        =
        let rec collect_moods_in_interval
            (changes: (Moment*Mood) array)
            current_index
            finish_index
            accumulated_mood
            =
            if current_index<=finish_index then
                collect_moods_in_interval
                    changes
                    (current_index+1)
                    finish_index
                    (
                        accumulated_mood+(
                            changes[current_index]|>snd
                        )
                    )
            else
                accumulated_mood
        collect_moods_in_interval
            changes
            start_index
            finish_index
            (Mood 0)

    let all_mood_changes_starting_from_index
        (changes: (Moment*Mood) array)
        (start_index:int)
        =
        let rec mood_changes_for_all_final_moments
            (changes: (Moment*Mood) array)
            (found_intervals: ResizeArray<Interval*Mood> )
            (start_index:int)
            (finish_index:int)
            =
            if finish_index<changes.Length then
                found_intervals.Add(
                    (
                        let moment_after_last_change =
                            changes[start_index]
                            |>fst
                            |>(+) 1
                        let moment_of_next_change =
                            (fst changes[finish_index])
                        Interval.regular
                            moment_after_last_change
                            moment_of_next_change
                    ),
                    (
                        mood_change_from_start_to_finish
                            changes
                            (start_index+1)
                            finish_index
                    )
                )
                mood_changes_for_all_final_moments
                    changes
                    found_intervals
                    start_index
                    (finish_index+1)
            else
                found_intervals
        mood_changes_for_all_final_moments
            changes
            (ResizeArray<Interval*Mood>())
            start_index
            (start_index+1)

    let one_mood_change_in_shortest_interval
        (changes: (Moment*Mood) array)
        (start_index:int)
        =
        (
            mood_change_from_start_to_finish
                changes
                start_index
                (start_index+1)
        )

    let intervals_changing_mood 
        (mood_changes_history: (Moment*Mood) seq)
        =
        let changes =
            mood_changes_history
            |>Seq.append [(-1,Mood 0)]//initial mood before the first change
            |>Array.ofSeq
            
        changes
        |>Seq.mapi (fun index _ ->
            all_mood_changes_starting_from_index
                changes
                index
        )
        |>Seq.collect (fun array->array.ToArray())
        
        


    [<Fact>]
    let ``try intervals_changing_mood``()=
        "1ok;23ok;45no2;67"
    //   01  234  567   89 <-moments
        |>built_from_text.Event_batches.event_batches_from_text
        |>Event_batches.only_mood_changes
        |>intervals_changing_mood
        |>should equal [
            (Interval.regular 0 1), Mood +1; 
            (Interval.regular 0 4), Mood +2;
            (Interval.regular 0 7), Mood 0;
            (Interval.regular 2 4), Mood +1; 
            (Interval.regular 2 7), Mood -1; 
            (Interval.regular 5 7), Mood -2;
        ]
    
    
        
    [<Fact>]
    let ``try intervals_changing_mood, with repeated signals``()=
        "00¬¬¬223××4¬¬"
    //   012  3456 78 9 <-moments
        |>built_from_text.Event_batches.event_batches_from_text
        |>Event_batches.only_mood_changes
        |>intervals_changing_mood
        |>should equal [
            (Interval.regular 0 2), Mood -3; 
            (Interval.regular 0 6), Mood -1;
            (Interval.regular 0 8), Mood -3;
            (Interval.regular 3 6), Mood +2; 
            (Interval.regular 3 8), Mood 0; 
            (Interval.regular 7 8), Mood -2;
        ]    