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
            if 
                current_index >= changes.Length 
                ||
                current_index > finish_index
            then
                accumulated_mood
            else
                changes[current_index]
                |>snd
                |>(+)accumulated_mood
                |>collect_moods_in_interval
                    changes
                    (current_index+1)
                    finish_index
           
        collect_moods_in_interval
            changes
            start_index
            finish_index
            (Mood 0)

    let private interval_from_indices
        (changes: (Moment*Mood) array)
        (start_index:int)
        (finish_index:int)
        =
        let start_moment =
            (fst changes[start_index])+1
        let moment_of_next_change =
            (fst changes[finish_index])
        Interval.regular
            start_moment
            moment_of_next_change
            
    let all_mood_changes_starting_from_index
        (changes: (Moment*Mood) array)
        (start_index:int)
        =
        let rec mood_changes_for_all_final_moments
            (changes: (Moment*Mood) array)
            (start_index:int)
            (finish_index:int)
            (found_intervals: (Interval*Mood) list )
            =
            if finish_index < changes.Length then
                (
                    interval_from_indices changes
                        start_index finish_index
                    ,
                    mood_change_from_start_to_finish
                        changes
                        start_index
                        finish_index
                )::found_intervals
                |>mood_changes_for_all_final_moments
                    changes
                    start_index
                    (finish_index+1)
            else
                found_intervals|>List.rev
        mood_changes_for_all_final_moments
            changes
            start_index
            start_index
            []

    let one_mood_change_in_shortest_interval
        (changes: (Moment*Mood) array)
        (start_index:int)
        =
        (
            interval_from_indices changes
                start_index (start_index+1)
            ,
            mood_change_from_start_to_finish
                changes
                start_index
                (start_index+1)
        )|>List.singleton

    let intervals_changing_mood 
        (
            mood_changes_starting_from_index: 
                (Moment*Mood) array -> int -> (Interval*Mood) list
        )
        (mood_changes_history: (Moment*Mood) seq)
        =
        let changes =
            mood_changes_history
            |>Seq.append [(-1,Mood 0)]//initial mood before the first change
            |>Array.ofSeq
            
        changes
        |>Seq.mapi (fun index _ ->
            mood_changes_starting_from_index
                changes
                index
        )
        |>Seq.concat
        
        


    [<Fact>]
    let ``try intervals_changing_mood``()=
        "0ok;23ok;45no2;67"
    //   0   12   34    5678 <-moments
        |>built_from_text.Event_batches.event_batches_from_text
            (built_from_text.Event_batches.mood_changes_as_words_and_numbers "no" "ok")
        |>Event_batches.only_mood_changes
        |>intervals_changing_mood 
            all_mood_changes_starting_from_index
        |>should equal [
            (Interval.moment 0), Mood +1; 
            (Interval.regular 0 2), Mood +2;
            (Interval.regular 0 4), Mood 0;
            (Interval.regular 1 2), Mood +1; 
            (Interval.regular 1 4), Mood -1; 
            (Interval.regular 3 4), Mood -2;
        ]
    
    
        
    [<Fact>]
    let ``try intervals_changing_mood, with repeated signals``()=
        "00¬¬¬223××4¬¬"
    //   01   234  5  6789 <-moments
        |>built_from_text.Event_batches.event_batches_from_text
            (built_from_text.Event_batches.mood_changes_as_repeated_symbols '¬' '×')
        |>Event_batches.only_mood_changes
        |>intervals_changing_mood 
            all_mood_changes_starting_from_index
        |>should equal [
            (Interval.regular 0 1), Mood -3; 
            (Interval.regular 0 4), Mood -1;
            (Interval.regular 0 5), Mood -3;
            (Interval.regular 2 4), Mood +2; 
            (Interval.regular 2 5), Mood 0; 
            (Interval.moment 5), Mood -2;
        ]  
    
    [<Fact>]
    let ``try intervals_changing_mood with one_mood_change_in_shortest_interval``()=
        "1ok;23ok;45no2;67"
    //   0   12   34    56789 <-moments
        |>built_from_text.Event_batches.event_batches_from_text
            (built_from_text.Event_batches.mood_changes_as_words_and_numbers "no" "ok")
        |>Event_batches.only_mood_changes
        |>intervals_changing_mood 
            one_mood_change_in_shortest_interval
        |>should equal [
            (Interval.moment 0), Mood +1; 
            (Interval.regular 1 2), Mood +1; 
            (Interval.regular 3 4), Mood -2;
        ]