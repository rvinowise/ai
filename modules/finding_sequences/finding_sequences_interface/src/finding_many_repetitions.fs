namespace rvinowise.ai



module Finding_many_repetitions =
    let many_repetitions = 
        ``Finding_many_repetitions(fsharp_simple)``.many_repetitions

    let all_repetitions =
        ``Finding_many_repetitions(fsharp_simple)``.all_repetitions

    let repetitions_in_combined_history
        (event_batches:Event_batches)
        =
        event_batches
        |>built.Event_batches.to_sequence_appearances
        |>many_repetitions
        |>built.Event_batches.from_sequence_appearances
        |>built.Event_batches.add_mood_to_combined_history
           (Event_batches.get_mood_history event_batches)
        |>built.Event_batches.remove_batches_without_actions