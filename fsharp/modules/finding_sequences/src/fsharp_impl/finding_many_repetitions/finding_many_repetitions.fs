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


    let will_this_pair_give_already_found_sequence 
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

    let many_repetitions
        (figure_appearances: seq<Figure_appearances>)
        =
        let known_sequences = 
            figure_appearances
            |>Seq.map (fun history->history.figure)
            |>Set.ofSeq
        
        (figure_appearances,figure_appearances)
        ||>Seq.allPairs
        |>Seq.fold (
            fun 
                (known_sequences, found_pairs)
                (a_history,b_history) 
                ->
            //test
            if a_history.figure|>Figure.id_of_a_sequence = "a" &&
               b_history.figure|>Figure.id_of_a_sequence = "da"
            then
                () 
            else
                () 
            ////test
            if will_this_pair_give_already_found_sequence 
                known_sequences
                a_history.figure
                b_history.figure
            then
                known_sequences,found_pairs
            else
                let found_pair = 
                    (a_history, b_history)
                    |>Finding_repetitions.repeated_pair_with_histories
                if Figure_appearances.has_repetitions found_pair then
                    (
                        known_sequences
                        |>Set.add found_pair.figure
                    ),
                    found_pair::found_pairs
                else
                    known_sequences,found_pairs
            )
            (known_sequences,[])
        |>snd

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
    
    [<Fact>]
    let ``try finding many_repetitions, when duplicating sequences are possible``()=
        let ad_figure = built.Figure.sequence_from_text "ad"
        let a_figure = built.Figure.signal "a"
        let da_figure = built.Figure.sequence_from_text "da"
        let ad_appearances = {
            Figure_appearances.figure=ad_figure
            appearances=[|
                0,2; 5,6
            |]|>Array.map Interval.ofPair
        }
        let a_appearances = {
            Figure_appearances.figure=a_figure
            appearances=[|
                0;5;8
            |]|>Array.map Interval.moment
        }
        let da_appearances = {
            Figure_appearances.figure=a_figure
            appearances=[|
                2,5; 6,8
            |]|>Array.map Interval.ofPair
        }
        let ada_appearances =
            [ad_appearances;a_appearances;da_appearances]
            |>many_repetitions
            |>Seq.filter (fun figure_appearances ->
                figure_appearances.figure = built.Figure.sequence_from_text "ada"
            )
        ada_appearances
        |>Seq.length
        |>should equal 1

        ada_appearances
        |>Seq.head
        |>(fun appearances -> appearances.appearances)
        |>should equal ([
            0,5; 5,8
        ]|>Seq.map Interval.ofPair)
