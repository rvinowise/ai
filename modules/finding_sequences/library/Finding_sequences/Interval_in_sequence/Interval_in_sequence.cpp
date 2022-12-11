#pragma once
#include "Interval_in_sequence.h"

namespace rvinowise::ai {


Interval_in_sequence::Interval_in_sequence():
_exists{false},
_interval{}
{}

Interval_in_sequence::Interval_in_sequence(
    size_t index, Interval interval
):
_index{index},
_interval{interval},
_exists{true}
{
    
}

Interval_in_sequence Interval_in_sequence::non_existent() {
    return Interval_in_sequence{};
}



}