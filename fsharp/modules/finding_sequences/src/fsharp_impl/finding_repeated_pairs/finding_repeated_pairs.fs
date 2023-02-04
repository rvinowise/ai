namespace rvinowise.ai

open System
open rvinowise.ai

open FsUnit
open Xunit


module Finding_repetitions =
    (* figure "a" is the beginning of the found pair, 
    and figure "b" is its ending, for intuitive naming *)

    type Cursor= {
        index:int
        moment:Moment
    }

    type Interval_in_sequence={
        exists:bool
        index: int
        interval:Interval
    }

    module Interval_in_sequence=
        let non_existent =
            {
                exists=false
                index=0
                interval=Interval.moment 0
            }

    type Iteration_state_of_searching_pairs={
        a_cursor: Interval_in_sequence
        b_cursor: Interval_in_sequence
        has_failed_to_find_pair: bool
    }

    module Iteration_state_of_searching_pairs=
        let found_pair state=
            Interval.regular
                state.a_cursor.interval.start
                state.b_cursor.interval.finish

        let not_found_state =
            {
                a_cursor=Interval_in_sequence.non_existent
                b_cursor=Interval_in_sequence.non_existent
                has_failed_to_find_pair = true
            }
        
        let state_before_the_first_iteration=
            {
                a_cursor={
                    exists=true
                    index=0
                    interval=Interval.moment 0
                }
                b_cursor={
                    exists=true
                    index=0
                    interval=Interval.moment 0
                }
                has_failed_to_find_pair=false
            }


    let rec next_half_of_pair
        (half_to_take: Interval->Moment)
        (start_from_index: int) // index right after the a-figure used for the previously found pair
        (start_from_moment: Moment) //start of the b-figure of the preiously found pair 
        (appearances: Interval array)
        =
        
        if (start_from_index >= appearances.Length) then
                Interval_in_sequence.non_existent
        else if (half_to_take appearances[start_from_index] >= start_from_moment) then
            {
                index=start_from_index
                interval=appearances[start_from_index]
                exists=true
            }
        else 
            next_half_of_pair 
                half_to_take
                (start_from_index+1)
                start_from_moment
                appearances

    let next_a
        (start_from_index: int) 
        (start_from_moment: Moment) // b-figure of the considered a-figure for the new appearance 
        (appearances_a: Interval array) 
        =
        next_half_of_pair
            Interval.finish
            start_from_index
            start_from_moment
            appearances_a

    let next_b
        (start_from_index: int) 
        (start_from_moment: Moment) // b-figure of the considered a-figure for the new appearance 
        (appearances_b: Interval array) 
        =
        next_half_of_pair
            Interval.start
            start_from_index
            start_from_moment
            appearances_b


    let rec find_A_closest_to_the_moment
        (appearances: Interval array)
        (* moment of the start of the considered B
        (the finish of the found A should go before it) *)
        (should_be_before_moment: Moment)
        (* the A found in the initial step of this iteration 
        (it can be the result) *)
        (start_from_index: int)
        (default_appearance: Interval_in_sequence)// =Interval_in_sequence.non_existent
        =
        if start_from_index < appearances.Length then
            let closer_appearance = 
                {
                    index=start_from_index
                    interval=appearances[start_from_index]
                    exists=true
                }
            if (closer_appearance.interval.finish >= should_be_before_moment) then
                default_appearance
            else
                find_A_closest_to_the_moment
                    appearances
                    should_be_before_moment
                    (closer_appearance.index+1)
                    closer_appearance
        else 
            default_appearance


    let iteration_of_finding_a_repeated_pair
        (previous_iteration: Iteration_state_of_searching_pairs)
        (a_appearances: Interval array)
        (b_appearances: Interval array)
        =
        // file://./iteration_of_finding_a_repeated_pair-get_next_headfigure.ora
        let next_a = 
            next_a
                previous_iteration.a_cursor.index 
                previous_iteration.b_cursor.interval.start
                a_appearances
        if (not next_a.exists) then
            Iteration_state_of_searching_pairs.not_found_state     
        else     
            // file://./iteration_of_finding_a_repeated_pair-find_next_tailfigure.ora
            let found_b = 
                next_b 
                    previous_iteration.b_cursor.index
                    (next_a.interval.finish+1)
                    b_appearances
                
            if (not found_b.exists) then
                Iteration_state_of_searching_pairs.not_found_state
            else
                // file://./iteration_of_finding_a_repeated_pair-find_previous_headfigure.ora
                let found_a = 
                    find_A_closest_to_the_moment
                        a_appearances
                        found_b.interval.start
                        next_a.index
                        Interval_in_sequence.non_existent

                if found_a.exists then
                    {
                        a_cursor=found_a
                        b_cursor=found_b
                        has_failed_to_find_pair=false
                    }
                else
                    Iteration_state_of_searching_pairs.not_found_state
        

    let rec find_repeated_pairs
        (a_appearances: Interval array)
        (b_appearances: Interval array)
        (previous_iteration: Iteration_state_of_searching_pairs)
        (found_pairs: Interval ResizeArray)
        =
        if (not previous_iteration.has_failed_to_find_pair) then
            found_pairs.Add(Iteration_state_of_searching_pairs.found_pair previous_iteration)
            let iteration = 
                iteration_of_finding_a_repeated_pair
                    previous_iteration
                    a_appearances
                    b_appearances

            find_repeated_pairs
                a_appearances
                b_appearances
                iteration
                found_pairs
        else
            found_pairs
    

    let repeated_pair 
        (a_appearances: Interval array)
        (b_appearances: Interval array)
        =
        let found_pairs = ResizeArray()

        let iteration = 
            iteration_of_finding_a_repeated_pair
                Iteration_state_of_searching_pairs.state_before_the_first_iteration
                a_appearances
                b_appearances

        find_repeated_pairs 
            a_appearances
            b_appearances
            iteration
            found_pairs
       
    [<Fact(Timeout=1000)>]
    let ``finding repetitions, when the last A-figure is taken, but there's still B-figures left ``()=
        async { 
            let signal1 = built.Figure_id_appearances.from_moments "signal1" [0;5]
            let signal2 = built.Figure_id_appearances.from_moments "signal2" [1;6;7]
            repeated_pair
                (signal1.appearances|>Array.ofSeq)
                (signal2.appearances|>Array.ofSeq)
            |>should equal (
                [
                    0,1; 5,6
                ]
                |>Seq.map Interval.ofPair
            )
        }
        
    let repeated_pair_with_histories
        (a_history: Figure_appearances,
        b_history: Figure_appearances)
        =
        {
            Figure_appearances.figure=
                built.Figure.sequential_pair
                    a_history.figure
                    b_history.figure
            appearances=
                (repeated_pair 
                    (a_history.appearances)
                    (b_history.appearances)
                ).ToArray()
        }


    [<Fact>]
    let ``try repeated_pair_with_histories``()=
        let a_history =
            built.Figure_id_appearances.from_moments "a" [0;5]
            |>built.Figure_appearances.from_figure_id_appearances
        let b_history =
            built.Figure_id_appearances.from_moments "b" [1;7]
            |>built.Figure_appearances.from_figure_id_appearances
                            
        repeated_pair_with_histories
            (a_history,b_history)
        |>should equal
            {
                Figure_appearances.figure={
                    edges=
                        ["a","b"]
                        |>Seq.map Edge.ofPair
                    subfigures=
                        ["a","a"; "b","b"]
                        |>Map.ofSeq
                }
                appearances=
                    [
                        0,1; 5,7
                    ]
                    |>Seq.map Interval.ofPair
                    |>Array.ofSeq
            }