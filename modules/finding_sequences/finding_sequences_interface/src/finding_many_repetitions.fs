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
    

    let all_repetitions =
        ``Finding_many_repetitions(simple)``.all_repetitions
           

    let repetitions_in_2_intervals =
        ``Finding_repetitions_across_intervals(simple)``.repetitions_in_2_intervals
        