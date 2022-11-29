#pragma once
#include "Iteration_state_of_searching_pairs.h"

namespace rvinowise::ai {



Iteration_state_of_searching_pairs::Iteration_state_of_searching_pairs():
_has_failed_to_find_pair(false) 
{}

Iteration_state_of_searching_pairs::Iteration_state_of_searching_pairs(
    Interval_in_sequence head_cursor,
    Interval_in_sequence tail_cursor
):
    head_cursor(head_cursor),
    tail_cursor(tail_cursor),
    _has_failed_to_find_pair(false)
{
    
}


Iteration_state_of_searching_pairs 
Iteration_state_of_searching_pairs::get_not_found_state() {
    Iteration_state_of_searching_pairs result;
    result._has_failed_to_find_pair = true;
    return result; 
}

Iteration_state_of_searching_pairs 
Iteration_state_of_searching_pairs::
get_state_before_the_first_iteration() {
    return Iteration_state_of_searching_pairs(
        Interval_in_sequence(
            0,
            Interval(0,0)
        ),
        Interval_in_sequence(
            0,
            Interval(0,0)
        )
    );
}

bool Iteration_state_of_searching_pairs::has_failed_to_find_pair() {
    return _has_failed_to_find_pair;
}
Interval Iteration_state_of_searching_pairs::get_found_pair() {
    return Interval(
        head_cursor.head(),
        tail_cursor.tail()
    );
}

}