namespace rvinowise.ai
    open FsUnit
    open Xunit
    open FSharpx.Collections
    open rvinowise

    type Mood_change_interval = {
        interval: Interval
        change: Mood
    }

    module Mood_change_interval=
        let from_tuples (tuples: (Moment*Moment*Mood) seq) =
            tuples
            |>Seq.map(fun (start,finish,mood)->
                {
                    interval=Interval.regular start finish
                    change=mood
                }
            )

    module Desiring_future=
        open rvinowise.ai

        let mood_change_from_start_to_finish
            (changes: (Moment*Mood) array)
            (start_index:int)
            (finish_index:int)
            =
            let rec collect_moods_in_interval
                (changes: (Moment*Mood) array)
                (current_index:int)
                (finish_index:int)
                (accumulated_mood: Mood)
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
                (changes: (Moment*Mood) array)
                (start_index:int)
                (finish_index:int)
                (Mood 0)

        let rec mood_changes_starting_from_index
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


        let intervals_changing_mood (mood_changes_history:Mood_changes_history)=
            let changes =
                mood_changes_history
                |>extensions.Map.toPairs
                |>Seq.append [(-1,Mood 0)]//initial mood before the first change
                |>Array.ofSeq
                
            changes
            |>Seq.mapi (fun index _ ->
                mood_changes_starting_from_index
                    changes
                    index
            )
            |>Seq.collect ResizeArray.toArray
            
            


        [<Fact>]
        let ``intervals changing mood``()=
            "1ok;23ok;45no2;67"
           //01  234  567   89 <-moments
            |>built_from_text.Event_batches.from_text
            |>Event_batches.to_separate_histories
            |>Separate_histories.mood_change_history
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
        let ``intervals changing mood (with multi-symbol good signals)``()=
            let good = "ok;"
            let bad = "bad;"
            "1ok;23;ok;45bad;bad;67"
//moment:    0123456789¹123456789²123456789
            |>built_from_text.Event_batches.from_text
            |>Event_batches.to_separate_histories
            |>Separate_histories.mood_change_history
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
        let ``intervals changing mood, with repeated signals``()=
            "00¬¬¬223××4¬¬"
           //012  3456 78 9 <-moments
            |>built_from_text.Event_batches.from_text
            |>Event_batches.to_separate_histories
            |>Separate_histories.mood_change_history
            |>intervals_changing_mood
            |>should equal [
                (Interval.regular 0 2), Mood -3; 
                (Interval.regular 0 6), Mood -1;
                (Interval.regular 0 8), Mood -3;
                (Interval.regular 3 6), Mood +2; 
                (Interval.regular 3 8), Mood 0; 
                (Interval.regular 7 8), Mood -2;
            ]


        
            

        let commonalities_between_two_intervals
            (interval1: Interval)
            (interval2: Interval)
            (figure_appearances: Figure_id_appearances seq)
            =
            figure_appearances
            |>Seq.map(fun figure_appearance->
                figure_appearance.appearances

            )

        let commonalities_in_history_intervals
            (is_interval_needed: Mood -> bool)
            (mood_intervals: (Interval*Mood) seq)
            (history: Figure_id_appearances seq)
            =
            ()


        let good_commonalities 
            (mood_intervals: (Interval*Mood) seq)
            (history: Figure_id_appearances seq)
            =
            commonalities_in_history_intervals
                (Mood.is_good)
                mood_intervals
                history


        let desired history=
            let separate_histories=
                history
                |>Event_batches.to_separate_histories
            let mood_intervals = 
                separate_histories
                |>Separate_histories.mood_change_history
                |>intervals_changing_mood
            // let good_commonalities =
            //     separate_histories
            //     |>good_commonalities mood_intervals
            ()