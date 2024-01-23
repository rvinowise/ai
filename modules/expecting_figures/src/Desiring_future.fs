namespace rvinowise.ai

open FsUnit
open Xunit
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

    let commonalities_between_two_intervals
        (interval1: Interval)
        (interval2: Interval)
        (figure_appearances)
        =
        ()

    let commonalities_in_history_intervals
        (is_interval_needed: Mood -> bool)
        (mood_intervals: (Interval*Mood) seq)
        (history)
        =
        ()


    let good_commonalities 
        (mood_intervals: (Interval*Mood) seq)
        (history)
        =
        commonalities_in_history_intervals
            (Mood.is_good)
            mood_intervals
            history


    let desired history=
        let mood_intervals = 
            history
            |>Event_batches.only_mood_changes
            |>Mood_history.intervals_changing_mood
                Mood_history.all_mood_changes_starting_from_index
        // let good_commonalities =
        //     separate_histories
        //     |>good_commonalities mood_intervals
        ()