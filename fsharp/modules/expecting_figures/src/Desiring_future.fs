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
                (start_index:int)
                (finish_index:int)
                (accumulated_mood: Mood)
                =
                if start_index<finish_index then
                    collect_moods_in_interval
                        changes
                        (start_index+1)
                        finish_index (
                            accumulated_mood+(
                                changes[start_index]|>snd
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
            //(found_intervals: ResizeArray<(Interval*Mood)> )
            =
            let rec mood_changes_for_all_final_moments
                (changes: (Moment*Mood) array)
                (found_intervals: ResizeArray<(Interval*Mood)> )
                (start_index:int)
                (finish_index:int)
                =
                if finish_index<changes.Length then
                    found_intervals.Add(
                        (Interval.regular start_index finish_index),
                        (
                            mood_change_from_start_to_finish
                                changes
                                start_index
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
                (ResizeArray<(Interval*Mood)>())
                start_index
                (start_index+1)


        let intervals_changing_mood (mood_changes_history:Mood_changes_history)=
            let changes =
                mood_changes_history
                |>extensions.Map.toPairs
                |>Array.ofSeq
            
            changes
            |>Seq.mapi (fun index _ ->
                mood_changes_starting_from_index
                    changes
                    index
            )
            |>Seq.collect (ResizeArray.toArray)
            
            


        [<Fact>]
        let ``intervals_changing_mood``()=
            "1×23×45¬¬67"
            //0123456789 <-moments
            |>built.Event_batches.from_text
            |>built.Event_batches.to_mood_changes_history
            |>intervals_changing_mood
            |>should equal [
                0,1,+1; 
                0,4,+2;
                0,7,0;
                2,4,+1; 
                2,7,-1; 
                5,7,-2;
            ]

        let desired history=
            ()
