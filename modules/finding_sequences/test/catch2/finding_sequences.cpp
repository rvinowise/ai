#define CATCH_CONFIG_MAIN
#include <catch2/catch_all.hpp>

#include "../Finding"

#include <vector>

using namespace std;
using namespace rvinowise::ai;

TEST_CASE("the same sequences") {
    
    vector<Interval> appearances_of_a{
        Interval(0,1),
        Interval(2,3),
        Interval(4,5),
        Interval(6,7)
    };

    vector<Interval> appearances_of_b{
        Interval(0,1),
        Interval(2,3),
        Interval(4,5),
        Interval(6,7)
    };

    vector<Interval> appearances_of_ab{
        Interval(0,3),
        Interval(4,7)
    };

    Finding_sequences finding_sequences();
    vector<Interval> found_pairs = 
        finding_sequences.find_repeated_pairs(appearances_of_a, appearances_of_b);

    ASSERT_EQ
}
