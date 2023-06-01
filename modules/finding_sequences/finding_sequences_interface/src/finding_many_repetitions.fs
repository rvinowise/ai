namespace rvinowise.ai

open System.IO
open System


module Reporting=
    let dont= fun _->()

module Reporting_repetitions=
    let write_to_file 
        filename
        (histories:Sequence_history_debug seq) 
        =
        use output_stream = File.AppendText(filename)
        histories
        |>Seq.sort
        |>Seq.iter (fun history->
            history
            |>output_stream.WriteLine
        )

module Finding_many_repetitions =
    let repetitions_of_one_stage = 
        //``Finding_many_repetitions(fsharp_dictionary_first)``.repetitions_of_one_stage
        ``Finding_many_repetitions(no_dictionary)``.repetitions_of_one_stage

    let all_repetitions 
        halves_can_form_pair
        report_findings
        (appearances: Sequence_appearances seq) 
        =
        appearances|>
        ``Finding_many_repetitions(no_dictionary)``.all_repetitions
            halves_can_form_pair
            //Reporting.dont
            report_findings
            
        //``Finding_many_repetitions(fsharp_dictionary_first)``.all_repetitions
    

    let repetitions_in_combined_history
        halves_can_form_pair
        (event_batches:Event_batches)
        =
        event_batches
        |>built.Event_batches.to_sequence_appearances
        |>repetitions_of_one_stage halves_can_form_pair
        |>built.Event_batches.from_sequence_appearances
        |>built.Event_batches.add_mood_to_combined_history
           (Event_batches.get_mood_history event_batches)
        |>built.Event_batches.remove_batches_without_actions