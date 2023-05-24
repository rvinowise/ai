namespace rvinowise.ai



module Finding_many_repetitions =
    let repetitions_of_one_stage = 
        ``Finding_many_repetitions(fsharp_simple)``.repetitions_of_one_stage
        //``Finding_many_repetitions(fsharp_no_dictionary)``.repetitions_of_one_stage

    let all_repetitions =
        ``Finding_many_repetitions(fsharp_simple)``.all_repetitions
        //``Finding_many_repetitions(fsharp_no_dictionary)``.all_repetitions

    let repetitions_in_combined_history
        (event_batches:Event_batches)
        =
        event_batches
        |>built.Event_batches.to_sequence_appearances
        |>repetitions_of_one_stage
        |>built.Event_batches.from_sequence_appearances
        |>built.Event_batches.add_mood_to_combined_history
           (Event_batches.get_mood_history event_batches)
        |>built.Event_batches.remove_batches_without_actions