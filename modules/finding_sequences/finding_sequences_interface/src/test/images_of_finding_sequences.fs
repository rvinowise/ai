namespace rvinowise.ai.test

open Xunit
open Xunit.Abstractions
open FsUnit
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running


module images_of_finding_sequences =
    open rvinowise.ai
    open rvinowise.ai
    open rvinowise.ui

    let repetitions_of_one_stage_in_combined_history
        (event_batches: Appearance_event list seq)
        =
        event_batches
        |>Event_batches.to_sequence_appearances
        |>Finding_many_repetitions.repetitions_of_one_stage Finding_repetitions.all_halves
        |>Appearances.sequence_appearances_to_id_appearances
        |>Event_batches.from_appearances
    let repetitions_of_one_stage = 
        Finding_many_repetitions.repetitions_of_one_stage
            Finding_repetitions.all_halves

    


    [<Fact(Skip="ui")>] //
    let ``visualising stages of pattern finding``()=
        let signal_history =
            [
                ["a";"b"]; //0
                ["c"]; //1
                ["d";"b"]; //2
                ["e"]; //3
                ["f"]; //4
                ["a"]; //5
                ["d";"e"]; //6
                ["e";"f";"g"]; //7
                ["a";"c"]; //8
            ]|>List.map (List.map (Figure_id>>Appearance_event.Signal))
        let step2_history =
            signal_history
            |>repetitions_of_one_stage_in_combined_history
        
        let step3_history =
            step2_history
            |>repetitions_of_one_stage_in_combined_history

        let step4_history =
            step3_history
            |>repetitions_of_one_stage_in_combined_history

        "stages of pattern search"
        |>infrastructure.Graph.empty
        |>infrastructure.Graph.with_filled_vertex "step 1"
            (ui.painted.History.add_combined_history signal_history)
        |>infrastructure.Graph.with_filled_vertex "step 2"
            (ui.painted.History.add_combined_history step2_history)
        |>infrastructure.Graph.with_filled_vertex "step 3"
            (ui.painted.History.add_combined_history step3_history)
        |>infrastructure.Graph.with_filled_vertex "step 4"
            (ui.painted.History.add_combined_history step4_history)
        |>ui.painted.image.open_image_of_graph



    [<Fact(Skip="ui")>] //
    let ``visualising stages of pattern finding, including all previous stages``()=
        let signal_history =
            [
                ["a";"b"];
                ["c"];
                ["d";"b"];
                ["e"];
                ["f"];
                ["a"];
                ["d";"e"];
                ["e";"f";"g"];
                ["a";"c"];
            ]|>Event_batches.event_history_from_lists
        let step2_sequence_histories =
            signal_history
            |>Event_batches.to_sequence_appearances
            |>repetitions_of_one_stage
        let step2_combined_history =
            signal_history
            |>Event_batches.add_sequence_appearances step2_sequence_histories

        let step3_sequence_histories =
            step2_sequence_histories
            |>repetitions_of_one_stage
        let step3_combined_history =
            step2_combined_history
            |>Event_batches.add_sequence_appearances step3_sequence_histories

        let step4_sequence_histories =
            step3_sequence_histories
            |>repetitions_of_one_stage
        let step4_combined_history =
            step3_combined_history
            |>Event_batches.add_sequence_appearances step4_sequence_histories

        "stages of pattern search with previous stages"
        |>infrastructure.Graph.empty
        |>infrastructure.Graph.with_filled_vertex "step 1"
            (ui.painted.History.add_combined_history signal_history)
        |>infrastructure.Graph.with_filled_vertex "step 2"
            (ui.painted.History.add_combined_history step2_combined_history)
        |>infrastructure.Graph.with_filled_vertex "step 3"
            (ui.painted.History.add_combined_history step3_combined_history)
        |>infrastructure.Graph.with_filled_vertex "step 4"
            (ui.painted.History.add_combined_history step4_combined_history)
        |>ui.painted.image.open_image_of_graph

    

    [<Fact(Skip="ui")>] //
    let ``visualising stages of pattern finding, with mood``()=
        let signal_history =
            [
                ["a";"b"];
                ["c"];
                ["+1";"d";"b"];
                ["e"];
                ["+2";"f"];
                ["a"];
                ["d";"e"];
                ["-3";"e";"f";"g"];
                ["a";"c"];
            ]|>Event_batches.event_history_from_lists
        let step2_history =
            signal_history
            |>repetitions_of_one_stage_in_combined_history
        
        let step3_history =
            step2_history
            |>repetitions_of_one_stage_in_combined_history

        let step4_history =
            step3_history
            |>repetitions_of_one_stage_in_combined_history

        "stages of pattern search, with mood"
        |>infrastructure.Graph.empty
        |>infrastructure.Graph.with_filled_vertex "step 1"
            (ui.painted.History.add_combined_history signal_history)
        |>infrastructure.Graph.with_filled_vertex "step 2"
            (ui.painted.History.add_combined_history step2_history)
        |>infrastructure.Graph.with_filled_vertex "step 3"
            (ui.painted.History.add_combined_history step3_history)
        |>infrastructure.Graph.with_filled_vertex "step 4"
            (ui.painted.History.add_combined_history step4_history)
        |>ui.painted.image.open_image_of_graph

    

        
        
    [<Fact(Skip="ui")>]
    let ``finding long overlaid sequences, not performance heavy``()=
        let original_signals =
            "abcdaefbgcdefg"
    //seq1:  abcd ef g
    //seq3:      a  b cdefg
    //mom:   0123456789¹123456789²123456789³
            |>built_from_text.Event_batches.event_batches_from_text
                built_from_text.Event_batches.no_mood
            |>Seq.map fst

        let found_sequences =
            original_signals 
            |>Event_batches.to_sequence_appearances
            |>Finding_many_repetitions.all_repetitions
                (Finding_repetitions.halves_are_close_enough 1)
                Reporting.dont
        let combined_found_sequences =
            found_sequences
            |>Appearances.sequence_appearances_to_id_appearances
            |>Event_batches.from_appearances


        "finding long overlaid sequences"
        |>infrastructure.Graph.empty
        |>infrastructure.Graph.with_filled_vertex "original signals"
            (ui.painted.History.add_combined_history original_signals)
        |>infrastructure.Graph.with_filled_vertex "found sequences"
            (ui.painted.History.add_combined_history combined_found_sequences)
        |>ui.painted.image.open_image_of_graph