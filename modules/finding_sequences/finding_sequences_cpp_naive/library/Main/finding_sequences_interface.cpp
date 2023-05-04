#include "../../include/finding_sequences_interface.h"
#include "Main.h"


#include <iostream>
#include <format>
#include <memory>


using namespace std;
using namespace rvinowise::ai;




extern "C"
{

DLLEXPORT void init_module(
) {
    Main::init_module();
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