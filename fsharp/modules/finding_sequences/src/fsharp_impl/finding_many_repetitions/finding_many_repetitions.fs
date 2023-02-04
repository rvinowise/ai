namespace rvinowise.ai

open Xunit
open FsUnit

open System
open rvinowise.ai
open rvinowise

module Finding_many_repetitions =
    

    let event_batches_to_signal_histories event_batches =
        event_batches
        |>built.Event_batches.to_separate_histories
        |>Separate_histories.figure_id_appearances
        |>Seq.map built.Figure_appearances.from_figure_id_appearances

    type Known_figures = Set<Figure>


    let is_needed_pair 
        (known_sequences: Known_figures)
        a_figure_id
        b_figure_id
        =
        let ab_figure =
            built.Figure.sequential_pair
                a_figure_id
                b_figure_id
        
        known_sequences
        |>Set.contains ab_figure 
        |>not

    let many_repetitions
        (figure_appearances: seq<Figure_appearances>)
        =
        let known_sequences = 
            figure_appearances
            |>Seq.map (fun history->history.figure)
            |>Set.ofSeq
        
        (figure_appearances,figure_appearances)
        ||>Seq.allPairs
        |>Seq.filter (fun (a_history,b_history)->
            is_needed_pair 
                known_sequences
                a_history.figure
                b_history.figure
        )
        |>Seq.map Finding_repetitions.repeated_pair_with_histories
        |>Seq.filter Figure_appearances.has_repetitions

    let repetitions_in_combined_history
        (event_batches:Event_batches)
        =
        event_batches
        |>event_batches_to_signal_histories
        |>many_repetitions
        |>built.Event_batches.from_figure_appearances
        |>built.Event_batches.add_mood_to_combined_history
           (Event_batches.get_mood_history event_batches)
        |>built.Event_batches.remove_batches_without_actions
    
    [<Fact>]
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
        |>event_batches_to_signal_histories
        |>many_repetitions
        |>Seq.map built.Figure_appearances.to_figure_id_appearances
        |>Seq.sort
        |>should equal [
            built.Figure_id_appearances.from_tuples "aa" [
                0,4; 4,6
            ]
            built.Figure_id_appearances.from_tuples "ab" [
                0,1; 6,7
            ]
            built.Figure_id_appearances.from_tuples "ac" [
                0,2; 4,5
            ]
            built.Figure_id_appearances.from_tuples "ca" [
                2,4; 5,6
            ]
        ]