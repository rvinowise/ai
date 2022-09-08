#include "../include/finding_sequences_interface.h"
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


DLLEXPORT void find_repeated_pairs(const char* head, const char* tail) {

    Main::get_instance()->find_repeated_pairs(head, tail);
    
}


}