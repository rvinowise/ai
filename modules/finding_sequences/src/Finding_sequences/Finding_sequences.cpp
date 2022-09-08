#include "Finding_sequences.h"

#include "./Interval_in_sequence/Interval_in_sequence.h"
#include "./Iteration_state_of_searching_pairs/Iteration_state_of_searching_pairs.h"
#include "Interval/Interval.h"

#include <iostream>
#include <format>
#include <memory>
#include <vector>



using namespace std;

namespace rvinowise::ai {

Finding_sequences::Finding_sequences() {

}


Finding_sequences::~Finding_sequences(){}



Interval_in_sequence get_next_headfigure(
    size_t start_from_index, // index right after the head used for the previously found appearance
    uint64_t start_from_moment, //head of the tail of the preiously found apperance 
    vector<Interval> head_appearances
) {
    
    for (size_t i = start_from_index; i < head_appearances.size(); ++i) {
        if (head_appearances[i].tail >= start_from_moment) {
            return Interval_in_sequence{
                i,
                head_appearances[i]
            };
        }
    }
    return Interval_in_sequence::non_existent();
}

Interval_in_sequence find_next_tailfigure(
    size_t start_from_index, 
    uint64_t start_from_moment, // tail of the considered head part for the new appearance
    vector<Interval> tail_appearances
) {
    for (size_t i = start_from_index; i < tail_appearances.size(); ++i) {
        if (tail_appearances[i].head >= start_from_moment) {
            return Interval_in_sequence{
                i,
                tail_appearances[i]
            };
        }
    }
    return Interval_in_sequence::non_existent();
}


Interval_in_sequence find_closest_appearance_to_the_moment(
    size_t start_from_index,
    uint64_t not_later_than_moment,
    vector<Interval> appearances
) {

    Interval_in_sequence considered_appearance =
        Interval_in_sequence(
            start_from_index,
            appearances[start_from_index]
        );
    if (considered_appearance.tail() > not_later_than_moment) {
        return Interval_in_sequence::non_existent();
    }
    while (
        considered_appearance.tail() < not_later_than_moment
        ) {
        considered_appearance = Interval_in_sequence(
            considered_appearance.index() + 1,
            appearances[considered_appearance.index() + 1]
        );
    }
    return considered_appearance;
}

Interval_in_sequence find_previous_headfigure(
    /* the head appearance found in the initial step of this iteration 
    (it can be the result) */
    size_t start_from_index, 

    /* moment of the head of the considered tail
    (the head of the found head should go after it) */
    uint64_t not_later_than_moment,
    vector<Interval> head_appearances
) {
    return find_closest_appearance_to_the_moment(
        start_from_index,
        not_later_than_moment,
        head_appearances
    );
}



Iteration_state_of_searching_pairs iteration_of_finding_a_repeated_pair(
    Iteration_state_of_searching_pairs iteration_conditions,
    vector<Interval> head_appearances, 
    vector<Interval> tail_appearances
) {
    // file://./iteration_of_finding_a_repeated_pair-get_next_headfigure.ora
    Interval_in_sequence next_headfigure = 
        get_next_headfigure( 
            iteration_conditions.head_cursor.index(), 
            iteration_conditions.tail_cursor.head(), 
            head_appearances
        ); 
        
    // file://./iteration_of_finding_a_repeated_pair-find_next_tailfigure.ora
    Interval_in_sequence found_tail = 
        find_next_tailfigure( 
            iteration_conditions.tail_cursor.index(), 
            next_headfigure.tail(), 
            tail_appearances
        );

    // file://./iteration_of_finding_a_repeated_pair-find_previous_headfigure.ora
    Interval_in_sequence found_head = 
        find_previous_headfigure( 
            next_headfigure.index(), 
            found_tail.head(), 
            head_appearances
        );

    if (found_head.exists() && found_tail.exists()) {
        return Iteration_state_of_searching_pairs(
            found_head,
            found_tail
        );
    }
    return Iteration_state_of_searching_pairs::get_not_found_state();
    
}


vector<Interval> Finding_sequences::find_repeated_pairs(
    vector<Interval> head_appearances, 
    vector<Interval> tail_appearances
) {
    

    vector<Interval> result;
    result.reserve(min(head_appearances.size(), tail_appearances.size()));

    Iteration_state_of_searching_pairs iteration = 
        Iteration_state_of_searching_pairs::get_state_before_the_first_iteration();

    do {
        iteration = iteration_of_finding_a_repeated_pair(
            iteration,
            head_appearances,
            tail_appearances
        );
        result.push_back(iteration.get_found_pair());
        
    } while (
        !iteration.has_failed_to_find_pair()
    );

    return result;
}


}