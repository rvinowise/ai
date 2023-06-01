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

    let repetitions_of_adjacent_sequences = 
        Finding_many_repetitions.repetitions_in_combined_history
            (Finding_repetitions.halves_are_close_enough 1)

    let repetitions_of_one_stage_of_adjacent_sequences = 
        Finding_many_repetitions.repetitions_of_one_stage
            (Finding_repetitions.halves_are_close_enough 1)

    [<Fact(Skip="ui")>] //
    let ``visualising stages of pattern finding``()=
        let signal_history =
            built.Event_batches.from_contingent_signals 0 [
                ["a";"b"];
                ["c"];
                ["d";"b"];
                ["e"];
                ["f"];
                ["a"];
                ["d";"e"];
                ["e";"f";"g"];
                ["a";"c"];
            ]
        let step2_history =
            signal_history
            |>repetitions_of_adjacent_sequences
        
        let step3_history =
            step2_history
            |>repetitions_of_adjacent_sequences

        let step4_history =
            step3_history
            |>repetitions_of_adjacent_sequences

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
            built.Event_batches.from_contingent_signals 0 [
                ["a";"b"];
                ["c"];
                ["d";"b"];
                ["e"];
                ["f"];
                ["a"];
                ["d";"e"];
                ["e";"f";"g"];
                ["a";"c"];
            ]
        let step2_sequence_histories =
            signal_history
            |>built.Event_batches.to_sequence_appearances
            |>repetitions_of_one_stage_of_adjacent_sequences
        let step2_combined_history =
            signal_history
            |>built.Event_batches.add_sequence_appearances step2_sequence_histories

        let step3_sequence_histories =
            step2_sequence_histories
            |>repetitions_of_one_stage_of_adjacent_sequences
        let step3_combined_history =
            step2_combined_history
            |>built.Event_batches.add_sequence_appearances step3_sequence_histories

        let step4_sequence_histories =
            step3_sequence_histories
            |>repetitions_of_one_stage_of_adjacent_sequences
        let step4_combined_history =
            step3_combined_history
            |>built.Event_batches.add_sequence_appearances step4_sequence_histories

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
    let ``visualising stages of pattern finding, mixing levels of abstraction``()=
        let signal_history =
            built.Event_batches.from_contingent_signals 0 [
                ["a";"b"];
                ["c"];
                ["d";"b"];
                ["e"];
                ["f"];
                ["a"];
                ["d";"e"];
                ["e";"f";"g"];
                ["a";"c"];
            ]
        let step1_sequence_histories = 
            signal_history
            |>built.Event_batches.to_sequence_appearances

        let step2_sequence_histories =
            step1_sequence_histories
            |>repetitions_of_one_stage_of_adjacent_sequences
            |>Seq.append step1_sequence_histories
        let step2_combined_history =
            step2_sequence_histories
            |>built.Event_batches.from_sequence_appearances 

        let step3_sequence_histories =
            step2_sequence_histories
            |>repetitions_of_one_stage_of_adjacent_sequences
            |>Seq.append step2_sequence_histories
        let step3_combined_history =
            step3_sequence_histories
            |>built.Event_batches.from_sequence_appearances

        let step4_sequence_histories =
            step3_sequence_histories
            |>repetitions_of_one_stage_of_adjacent_sequences
            |>Seq.append step3_sequence_histories
        let step4_combined_history =
            step4_sequence_histories
            |>built.Event_batches.from_sequence_appearances

        "stages of pattern search mixing all layers of abstraction"
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
            built.Event_batches.from_contingent_signals 0 [
                ["a";"b"];
                ["c"];
                ["+1";"d";"b"];
                ["e"];
                ["+2";"f"];
                ["a"];
                ["d";"e"];
                ["-3";"e";"f";"g"];
                ["a";"c"];
            ]
        let step2_history =
            signal_history
            |>repetitions_of_adjacent_sequences
        
        let step3_history =
            step2_history
            |>repetitions_of_adjacent_sequences

        let step4_history =
            step3_history
            |>repetitions_of_adjacent_sequences

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
            |>built.Event_batches.from_text
        
        let found_sequences =
            original_signals 
            |>built.Event_batches.to_sequence_appearances
            |>Finding_many_repetitions.all_repetitions
                (Finding_repetitions.halves_are_close_enough 1)
                Reporting.dont
        let combined_found_sequences =
            found_sequences
            |>built.Event_batches.from_sequence_appearances


        "finding long overlaid sequences"
        |>infrastructure.Graph.empty
        |>infrastructure.Graph.with_filled_vertex "original signals"
            (ui.painted.History.add_combined_history original_signals)
        |>infrastructure.Graph.with_filled_vertex "found sequences"
            (ui.painted.History.add_combined_history combined_found_sequences)
        |>ui.painted.image.open_image_of_graph