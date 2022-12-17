#include "../../include/finding_sequences_interface.h"
#include "Main.h"


#include <iostream>
#include <format>
#include <memory>

#include <soci/soci.h>
#include <soci/postgresql/soci-postgresql.h>

using namespace std;
using namespace rvinowise::ai;




extern "C"
{

DLLEXPORT void init_module(
    const char* connection_string
) {
    Main::init_module(connection_string);
}


DLLEXPORT void find_repeated_pairs_in_database(const char* head, const char* tail) {

    Main::get_instance()->find_repeated_pairs_in_database(head, tail);
    
}

DLLEXPORT size_t find_repeated_pairs(
    Interval heads[], 
    size_t size_heads, 
    Interval tails[],
    size_t size_tails,
    Interval repetitions[]
) {

    vector<Interval> result = Finding_sequences::find_repeated_pairs(
        vector<Interval>(heads, heads+size_heads), 
        vector<Interval>(tails, tails+size_tails)
    );

    repetitions = new Interval[result.size()];
    for(size_t i=0; i<result.size(); i++) {
        repetitions[i] = result[i];
    }

    return result.size();
}


DLLEXPORT void release_output_memory(
    Interval pArray[]
) {
    delete[] pArray;
}

}