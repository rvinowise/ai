namespace rvinowise.ai.test

open Xunit
open Xunit.Abstractions
open FsUnit
open BenchmarkDotNet.Configs
open BenchmarkDotNet.Attributes
open BenchmarkDotNet.Running


module finding_sequences_in_history =
    open rvinowise.ai
    open rvinowise.ai
    open rvinowise.ui

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
            |>Finding_many_repetitions.repetitions_in_combined_history
        
        let step3_history =
            step2_history
            |>Finding_many_repetitions.repetitions_in_combined_history

        let step4_history =
            step3_history
            |>Finding_many_repetitions.repetitions_in_combined_history

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
        let step2_figure_histories =
            signal_history
            |>built.Event_batches.to_figure_appearances
            |>Finding_many_repetitions.many_repetitions
        let step2_combined_history =
            signal_history
            |>built.Event_batches.add_figure_appearances step2_figure_histories

        let step3_figure_histories =
            step2_figure_histories
            |>Finding_many_repetitions.many_repetitions
        let step3_combined_history =
            step2_combined_history
            |>built.Event_batches.add_figure_appearances step3_figure_histories

        let step4_figure_histories =
            step3_figure_histories
            |>Finding_many_repetitions.many_repetitions
        let step4_combined_history =
            step3_combined_history
            |>built.Event_batches.add_figure_appearances step4_figure_histories

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

    [<Fact>] //(Skip="ui")
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
        let step1_figure_histories = 
            signal_history
            |>built.Event_batches.to_figure_appearances

        let step2_figure_histories =
            step1_figure_histories
            |>Finding_many_repetitions.many_repetitions
            |>Seq.append step1_figure_histories
        let step2_combined_history =
            step2_figure_histories
            |>built.Event_batches.from_figure_appearances 

        let step3_figure_histories =
            step2_figure_histories
            |>Finding_many_repetitions.many_repetitions
            |>Seq.append step2_figure_histories
        let step3_combined_history =
            step3_figure_histories
            |>built.Event_batches.from_figure_appearances

        let step4_figure_histories =
            step3_figure_histories
            |>Finding_many_repetitions.many_repetitions
            |>Seq.append step3_figure_histories
        let step4_combined_history =
            step4_figure_histories
            |>built.Event_batches.from_figure_appearances

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
            |>Finding_many_repetitions.repetitions_in_combined_history
        
        let step3_history =
            step2_history
            |>Finding_many_repetitions.repetitions_in_combined_history

        let step4_history =
            step3_history
            |>Finding_many_repetitions.repetitions_in_combined_history

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

    [<Fact>]
    let ``finding long overlaid sequences``()=
        let original_signals =
            "a1bc2d31a2ef4bg3c54de6fh5g6h"
    //seq1:  a bc d    ef  g
    //seq2:   1  2 3     4    5   6  
    //seq3:          a    b  c  de f  g h         
    //seq4:         1 2     3  4     5 6  
    //mom:   123456789¹123456789²123456789³
            |>built.Event_batches.from_text
        
        let found_sequences =
            original_signals 
            |>built.Event_batches.to_figure_appearances
            |>Finding_many_repetitions.all_repetitions
        let combined_found_sequences =
            found_sequences
            |>built.Event_batches.from_figure_appearances


        "finding long overlaid sequences"
        |>infrastructure.Graph.empty
        |>infrastructure.Graph.with_filled_vertex "original signals"
            (ui.painted.History.add_combined_history original_signals)
        |>infrastructure.Graph.with_filled_vertex "found sequences"
            (ui.painted.History.add_combined_history combined_found_sequences)
        |>ui.painted.image.open_image_of_graph