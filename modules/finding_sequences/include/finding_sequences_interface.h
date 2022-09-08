#pragma once

#include "../src/preprocessor_directives.h"


extern "C"
{

DLLEXPORT void init_module(
    const char* db_connection
);

DLLEXPORT void find_repeated_pairs(
    const char* head, const char* tail
);


}