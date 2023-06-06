namespace rvinowise.ai

open System.IO
open System


module Reporting=
    let dont= fun _->()

module Reporting_repetitions=
    let write_to_file 
        filename
        (histories: (Sequence*Interval array) seq) 
        =
        use output_stream = File.AppendText(filename)
        histories
        |>Seq.sort
        |>Seq.iter (fun history->
            history
            |>Appearances.sequence_appearances_to_string
            |>output_stream.WriteLine
        )

module Finding_many_repetitions =
    let repetitions_of_one_stage = 
        //``Finding_many_repetitions(fsharp_dictionary_first)``.repetitions_of_one_stage
        ``Finding_many_repetitions(simple)``.repetitions_of_one_stage
    
    


    let all_repetitions 
        halves_can_form_pair
        report_findings
        appearances
        =
        appearances|>
        ``Finding_many_repetitions(simple)``.all_repetitions
            halves_can_form_pair
            report_findings
            


    let repetitions_across_intervals
        halves_can_form_pair
        interval1_appearances
        interval2_appearances
        =
        ``Finding_many_repetitions_across_intervals(simple)``.repetitions_across_intervals
            halves_can_form_pair
            interval1_appearances
            interval2_appearances
        