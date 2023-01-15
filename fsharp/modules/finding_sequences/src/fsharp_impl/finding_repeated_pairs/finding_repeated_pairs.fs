namespace rvinowise.ai.fsharp_impl

open System
open rvinowise.ai

module Finding_repeated_pairs =
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

    let next_a
        (start_from_index: int) // index right after the a-figure used for the previously found pair
        (start_from_moment: Moment) //start of the b-figure of the preiously found pair 
        (appearances_a: Interval array)
        =
        
        for i in start_from_index .. appearances_a.Length-1 do
            if (appearances_a[i].finish >= start_from_moment) then
                {
                    index=i
                    interval=appearances_a[i]
                    exists=true
                }
            else
                ()
        Interval_in_sequence.non_existent
    }





    let cursor_from_finish_a 
        index
        (appearance_a:Interval) 
        = {
            index=index
            moment=appearance_a.finish
        }

    let next_pair
        (appearances_a: Interval array)
        (appearances_b: Interval array)
        (cursor: Cursor)
        =
        let next_a = appearances_a[cursor.index+1]
        let next_b = appearances_b[cursor.index+1]
            

    let repeated_pair 
        (appearances_a: Interval array)
        (appearances_b: Interval array)
        =
        a_appearances
        |>Seq.mapi cursor_from_finish_a
        |>Seq.map next_pair a_appearances b_appearances


