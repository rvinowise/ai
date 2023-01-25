namespace rvinowise.ai.fsharp_impl

open Xunit
open FsUnit

open System
open rvinowise.ai
open rvinowise

module Finding_many_repetitions =

    let is_possible_pair
        (a_history,b_history)
        =
        if a_history.figure = b_history.figure then
            false
        else
            true

    let many_repetitions
        (figure_histories: seq<Figure_history>)
        =
        (figure_histories,figure_histories)
        ||>Seq.allPairs
        //|>Seq.filter is_possible_pair
        |>Seq.map Finding_repetitions.repeated_pair_with_histories
        |>Seq.filter Figure_history.has_repetitions

    let repetitions_in_combined_history
        (event_batches:Event_batches)
        =
        event_batches
        |>built.Event_batches.to_separate_histories
        |>Separate_histories.figure_histories
        |>many_repetitions
        |>built.Event_batches.combine_figure_histories
        |>built.Event_batches.add_mood_to_combined_history
           (Event_batches.get_mood_history event_batches)
        |>built.Event_batches.remove_batches_without_actions
    
    [<Fact>]//(Skip="bug")
    let ``finding repetitions in simple combined history``()=
        built.Event_batches.from_contingent_signals 0 [
            ["a"];//0
            ["b"];//1
            ["c"];//2
            ["x"];//3
            ["a"];//4
            ["c"];//5
            ["a"];//6
            ["b"];//7
        ]
        |>built.Event_batches.to_separate_histories
        |>Separate_histories.figure_histories
        |>many_repetitions
        |>Seq.sort
        |>should equal [
            built.Figure_history.from_tuples "aa" [
                0,4; 4,6
            ]
            built.Figure_history.from_tuples "ab" [
                0,1; 6,7
            ]
            built.Figure_history.from_tuples "ac" [
                0,2; 4,5
            ]
            built.Figure_history.from_tuples "ca" [
                2,4; 5,6
            ]
        ]