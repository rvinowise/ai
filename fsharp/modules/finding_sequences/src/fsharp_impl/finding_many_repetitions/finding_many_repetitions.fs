namespace rvinowise.ai

open Xunit
open FsUnit

open System
open rvinowise.ai
open rvinowise

module Finding_many_repetitions =
    

    type Known_figures = Set<Figure>


    let private will_this_pair_give_already_found_sequence 
        (known_sequences: Known_figures)
        a_figure_id
        b_figure_id
        =
        let ab_figure =
            built.Fusing_figures_into_sequence.sequential_pair
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
               ||
               a_history.figure|>Figure.id_of_a_sequence = "ad" &&
               b_history.figure|>Figure.id_of_a_sequence = "a"
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
        |>built.Event_batches.to_figure_appearances
        |>many_repetitions
        |>built.Event_batches.from_figure_appearances
        |>built.Event_batches.add_mood_to_combined_history
           (Event_batches.get_mood_history event_batches)
        |>built.Event_batches.remove_batches_without_actions
    
    [<Fact>]
    let ``try finding many_repetitions, in simple combined history``()=
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
        |>built.Event_batches.to_figure_appearances
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
            Figure_appearances.figure=da_figure
            appearances=[|
                2,5; 6,8
            |]|>Array.map Interval.ofPair
        }
        let test_ada = built.Figure.sequence_from_text "ada"
        let ada_appearances =
            [ad_appearances;a_appearances;da_appearances]
            |>many_repetitions
            |>Seq.filter (fun figure_appearances ->
                figure_appearances.figure = built.Figure.sequence_from_text "ada"
            )
        ada_appearances
        |>List.ofSeq
        |>should haveLength 1

        ada_appearances
        |>Seq.head
        |>(fun appearances -> appearances.appearances)
        |>should equal ([
            0,5; 5,8
        ]|>Seq.map Interval.ofPair)


    let all_repetitions 
        (figure_appearances: seq<Figure_appearances>)
        =
        let rec steps_of_finding_repetitions
            (all_sequences: seq<Figure_appearances>)
            (sequences_of_previous_step: seq<Figure_appearances>)
            =
            if Seq.isEmpty sequences_of_previous_step then
                all_sequences
            else
                let all_sequences =
                    all_sequences
                    |>Seq.append sequences_of_previous_step
                all_sequences
                |>many_repetitions
                |>steps_of_finding_repetitions (
                    all_sequences
                )
        
        steps_of_finding_repetitions
            []
            figure_appearances

    [<Fact>]
    let ``try finding all_repetitions (several levels of abstraction)``()=
        let ab_history = {
            Figure_appearances.figure=built.Figure.sequence_from_text "ab"
            appearances=[|
                0,2; 6,9
            |]|>Array.map Interval.ofPair
        }
        let ac_history = {
            Figure_appearances.figure=built.Figure.sequence_from_text "ac"
            appearances=[|
                0,4; 6,10
            |]|>Array.map Interval.ofPair
        }
        let bc_history = {
            Figure_appearances.figure=built.Figure.sequence_from_text "bc"
            appearances=[|
                2,4; 9,10
            |]|>Array.map Interval.ofPair
        }
        let abc_history = {
            Figure_appearances.figure=built.Figure.sequence_from_text "abc"
            appearances=[|
                0,4; 6,10
            |]|>Array.map Interval.ofPair
        }
        let expected_sequences = 
            Set.ofList [
                ab_history;ac_history;bc_history;abc_history
            ]
        
        "a1b2c3a45bc"
//       a b c
//             a  bc
//mom:   0123456789¹1
        |>built.Event_batches.from_text
        |>built.Event_batches.to_figure_appearances
        |>all_repetitions
        |>Set.ofSeq
        |>Set.intersect expected_sequences
        |>should equal expected_sequences