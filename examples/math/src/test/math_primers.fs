namespace rvinowise.ai.math

open Xunit
open FsUnit
open System.IO

open rvinowise.ai
open rvinowise
open rvinowise.ui

module Math_primers=
    open System


    [<Fact>]
    let ``find sequences in math primers``()=
        [
            "N:0,1,2,3,4,5,6,7,8,9;";"1+1=2;ok;";"1+2=3;ok;";"1+1=";
//mom:       0123456789¹123456789²1   2345678     9³12345     6789
//seq1                                 +1          +          1
//seq2                                             +          1+1
//mom(20+):  2345678    9³12345
//mom(30+):  6789
        ]
        |>built_from_text.Event_batches.from_text_blocks
        |>Event_batches.only_signals
        |>Event_batches.to_sequence_appearances
        |>Finding_many_repetitions.all_repetitions
            (Finding_repetitions.halves_are_close_enough 1)
            Reporting.dont
        |>Set.ofSeq
        |>should be (supersetOf(
            [
                "1+1="|>Sequence.ofString, [22,25;36,39]|>List.map Interval.ofPair;
                "1+=;"|>Sequence.ofString, [22,27;29,34]|>List.map Interval.ofPair;
            ]|>Set.ofSeq
        ))

    [<Fact(Skip="ui")>]
    let ``draw training history``()=
        let input_primers =
            built_from_text.Event_batches.from_text_blocks [
                "N:0,1,2,3,4,5,6,7,8,9;";
                "1+1=2;ok;"; "1+2=3;ok;";
                "1+1=";
            ]
            |>Event_batches.only_signals
        
        "math inputs"
        |>infrastructure.Graph.empty
        |>infrastructure.Graph.with_filled_vertex "initial signals"
            (ui.painted.History.add_combined_history input_primers)
        |>ui.painted.image.open_image_of_graph

    [<Fact>]
    let ``ai can reply with rote-memorised constant sequences``()=
        built_from_text.Event_batches.from_text_blocks [
            "N:0,1,2,3,4,5,6,7,8,9;";
            "1+1=2;×"; "1+2=3;×";
            "1+1=";
        ]
        |>ai.Desiring_future.desired
        |>should equal [
            ("2",";");
            (";","×");
        ]

    [<Fact>]//(Skip="ui")
    let ``find repetitions which lead to good``()=
        let good_signal = "ok;"
        use input_stream =
            new StreamReader "C:/prj/ai/examples/math/mathematical_primers.txt"
        let raw_signals =
            input_stream.ReadToEnd()
            |>built_from_text.Event_batches.event_batches_from_text
            |>Event_batches.only_signals
            |>Event_batches.to_sequence_appearances
        
        let intermediat_results_file = @"C:\prj\ai\examples\math\mathematical_primers_intermediate_output.txt"
        let final_results_file ="C:/prj/ai/examples/math/math_output.txt"
        File.Delete(intermediat_results_file)
        File.Delete(final_results_file)
        use output_stream = File.AppendText(final_results_file)
        
        let all_repetitions =
            raw_signals
            |>Finding_many_repetitions.all_repetitions
                (Finding_repetitions.halves_are_close_enough 1)
                (
                Reporting_repetitions.write_to_file 
                    intermediat_results_file
                )
        
        output_stream.WriteLine $"sequences ending with {good_signal}"
        all_repetitions
        |>Seq.filter (fun appearances ->
            appearances|>fst
            |>Array.rev
            |>Array.truncate (String.length good_signal)
            |>Array.rev
            |>Seq.map Figure_id.value
            |>String.concat ""
            |>(=)good_signal
        )|>Seq.map Appearances.sequence_appearances_to_string 
        |>Seq.iter output_stream.WriteLine

        output_stream.WriteLine $"sequences containing {good_signal}"
        all_repetitions
        |>Seq.filter (fun appearances ->
            appearances|>fst
            |>Seq.map Figure_id.value
            |>String.concat ""
            |>(fun string -> string.Contains(good_signal))
        )|>Seq.map Appearances.sequence_appearances_to_string 
        |>Seq.iter output_stream.WriteLine