#include "../include/finding_sequences.h"

#include <stdio.h>

extern "C"
{

DLLEXPORT void find_repeated_pairs(
    const char* db_connection,
    int a,
    double b
) {
    printf("You called method find_repeated_pairs(), You passed in %s %d %f\n\r",
        db_connection, a,b
    );
}


}