#pragma once

#include "../library/preprocessor_directives.h"
#include "Interval/Interval.h"
#include <string>

extern "C"
{

DLLEXPORT void init_module(
);


DLLEXPORT size_t find_repeated_pairs(
    rvinowise::ai::Interval heads[], 
    size_t size_heads, 
    rvinowise::ai::Interval tails[],
    size_t size_tails,
    rvinowise::ai::Interval repetitions[]
);

DLLEXPORT void release_output_memory(
    rvinowise::ai::Interval pArray[]
);


}