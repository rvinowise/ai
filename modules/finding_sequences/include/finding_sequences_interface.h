#pragma once

#include "../library/preprocessor_directives.h"
#include "Interval/Interval.h"
#include <string>

extern "C"
{

DLLEXPORT void init_module(
    const char* db_connection
);

DLLEXPORT void find_repeated_pairs_in_database(
    const char* head, const char* tail
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