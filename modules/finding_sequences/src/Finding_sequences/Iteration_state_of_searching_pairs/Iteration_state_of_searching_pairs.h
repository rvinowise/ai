#pragma once

#include "Interval/Interval.h"
#include "../Interval_in_sequence/Interval_in_sequence.h"


namespace rvinowise::ai {

class Iteration_state_of_searching_pairs {
    
    private:
    
    bool _has_failed_to_find_pair;
    Iteration_state_of_searching_pairs();

    public:
    
    Iteration_state_of_searching_pairs(
        Interval_in_sequence head_cursor,
        Interval_in_sequence tail_cursor
    );
 
    static Iteration_state_of_searching_pairs get_state_before_the_first_iteration();
    static Iteration_state_of_searching_pairs get_not_found_state();

    Interval get_found_pair();
    bool has_failed_to_find_pair();
    
    Interval_in_sequence tail_cursor;
    Interval_in_sequence head_cursor;

};

}