namespace rvinowise.ai


module ``Finding_repetitions(fsharp_simple)`` =
    (* figure "a" is the beginning of the found pair, 
    and figure "b" is its ending, for intuitive naming *)


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

    type Result_of_iteration =
        | Pair_found
        | Not_found_yet
        | Reached_end
    type Iteration_state_of_searching_pairs={
        a_cursor: Interval_in_sequence
        b_cursor: Interval_in_sequence
        result: Result_of_iteration
    }

    module Iteration_state_of_searching_pairs=
        let found_pair state=
            Interval.regular
                state.a_cursor.interval.start
                state.b_cursor.interval.finish

        let reached_end_state = {
            a_cursor=Interval_in_sequence.non_existent
            b_cursor=Interval_in_sequence.non_existent
            result = Reached_end
        }
        let pair_not_found_state = {
            a_cursor=Interval_in_sequence.non_existent
            b_cursor=Interval_in_sequence.non_existent
            result = Not_found_yet
        }
        
        let state_before_the_first_iteration={
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
            result = Pair_found
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
        (a_appearances: Interval array) 
        =
        next_half_of_pair
            Interval.finish
            start_from_index
            start_from_moment
            a_appearances

    let next_b
        (start_from_index: int) 
        (start_from_moment: Moment) // b-figure of the considered a-figure for the new appearance 
        (b_appearances: Interval array) 
        =
        next_half_of_pair
            Interval.start
            start_from_index
            start_from_moment
            b_appearances


    let rec find_A_closest_to_the_moment
        (appearances: Interval array)
        (* moment of the start of the considered B
        (the finish of the found A should go before it) *)
        (moment_of_b: Moment)
        (* the A found in the initial step of this iteration 
        (it can be the result) *)
        (start_from_index: int)
        (default_appearance: Interval_in_sequence)// =Interval_in_sequence.non_existent
        =
        if start_from_index < appearances.Length then
            let closer_appearance = {
                index=start_from_index
                interval=appearances[start_from_index]
                exists=true
            }
            if (closer_appearance.interval.finish < moment_of_b) then
                find_A_closest_to_the_moment
                    appearances
                    moment_of_b
                    (closer_appearance.index+1)
                    closer_appearance
            else
                default_appearance
        else 
            default_appearance
        
    let find_suitable_A    
        (a_appearances: Interval array)
        (moment_of_b: Moment)
        (start_from_index: int)
        (default_appearance: Interval_in_sequence)
        =
        let closest_a = 
            find_A_closest_to_the_moment
                a_appearances
                moment_of_b
                start_from_index
                default_appearance
        
        let maximum_distance_between_a_and_b = 1 
        if closest_a.exists then 
            let distance_to_b = 
                moment_of_b - closest_a.interval.finish
            if (distance_to_b <= maximum_distance_between_a_and_b) then
                closest_a
            else
                Interval_in_sequence.non_existent
        else
            Interval_in_sequence.non_existent

    

    let iteration_of_finding_a_repeated_pair
        (halves_can_form_pair: Interval->Interval->bool)
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
        if (next_a.exists) then
            // file://./iteration_of_finding_a_repeated_pair-find_next_tailfigure.ora
            let found_b = 
                next_b 
                    previous_iteration.b_cursor.index
                    (next_a.interval.finish+1)
                    b_appearances
                
            if (found_b.exists) then
                // file://./iteration_of_finding_a_repeated_pair-find_previous_headfigure.ora + consider the distance between A and B
                let closest_a = 
                    find_A_closest_to_the_moment
                        a_appearances
                        found_b.interval.start
                        (next_a.index+1)
                        next_a
                {
                    a_cursor=closest_a
                    b_cursor=found_b
                    result=
                        if halves_can_form_pair closest_a.interval found_b.interval then
                            Pair_found
                        else
                            Not_found_yet
                }
            else
                Iteration_state_of_searching_pairs.reached_end_state
        else     
            Iteration_state_of_searching_pairs.reached_end_state     
        

    let rec find_repeated_pairs
        (halves_can_form_pair: Interval->Interval->bool)
        (a_appearances: Interval array)
        (b_appearances: Interval array)
        (previous_iteration: Iteration_state_of_searching_pairs)
        (found_pairs: Interval list)
        =
        let updated_pairs = 
            match previous_iteration.result with
            |Pair_found->
                (Iteration_state_of_searching_pairs.found_pair previous_iteration)
                ::found_pairs
            |_-> found_pairs
        
        match previous_iteration.result with
        |Pair_found|Not_found_yet->
            let iteration = 
                iteration_of_finding_a_repeated_pair
                    halves_can_form_pair
                    previous_iteration
                    a_appearances
                    b_appearances

            find_repeated_pairs
                halves_can_form_pair
                a_appearances
                b_appearances
                iteration
                updated_pairs
        |Reached_end->
            found_pairs|>List.rev

    let repeated_pair 
        (halves_can_form_pair: Interval->Interval->bool)
        (a_appearances: Interval array)
        (b_appearances: Interval array)
        =
        let iteration = 
            iteration_of_finding_a_repeated_pair
                halves_can_form_pair
                Iteration_state_of_searching_pairs.state_before_the_first_iteration
                a_appearances
                b_appearances

        find_repeated_pairs 
            halves_can_form_pair
            a_appearances
            b_appearances
            iteration
            []
        |>Array.ofList
        
    let repeated_pair_of_sequences
        (halves_can_form_pair: Interval->Interval->bool)
        (histories: (Sequence*Interval array)*(Sequence*Interval array))
        =
        let (
                a_sequence,
                a_appearances
            ),
            (
                b_sequence,
                b_appearances
            ) = histories
        let ab_sequence = 
            Array.append
                a_sequence
                b_sequence

        let ab_appearances = 
            repeated_pair
                halves_can_form_pair
                a_appearances
                b_appearances
        
        ab_sequence, ab_appearances
      

