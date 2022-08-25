#define CATCH_CONFIG_MAIN  // This tells Catch to provide a main() - only do this in one cpp file
#include "catch.hpp"

unsigned int Factorial( unsigned int number ) {
    return number <= 1 ? number : Factorial(number-1)*number;
}

TEST_CASE( "connected to the database", "[factorial]" ) {
    REQUIRE( 
        find_repeated_pairs("postgresql://postgres:123@127.0.0.1:5432/ai") == 1 
    );
    
}