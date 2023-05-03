//#define CATCH_CONFIG_MAIN

#include "Finding_sequences/Finding_sequences.h"

#include <catch2/catch_all.hpp>
#include <vector>
#include <iostream>

using namespace std;
using namespace rvinowise::ai;



TEST_CASE(
    "the same sequences\n"
    "there's no tail appearance after the last head appearance"
) {
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
        Interval(2,5),
        Interval(4,7)
    };
    // several appearances are found
    Finding_sequences finding_sequences;
    vector<Interval> found_pairs = 
       finding_sequences.find_repeated_pairs(appearances_of_a, appearances_of_b);

    REQUIRE(found_pairs == appearances_of_ab);
}

TEST_CASE(
    "several heads before the next tail;\n"
    "the last head in the array is before the last tail in the array\n"
) {
    vector<Interval> appearances_of_a{
        //1st iteration
        Interval(0,1), //skipped as not closest
        Interval(2,3), //used as the head
        Interval(4,5), //ignored as not being before the found tail
        //2nd iteration
        //Interval(4,5), //ignored as not being after the last tail
        Interval(6,7), //skipped as not closest
        Interval(8,9), //skipped as not closest
        Interval(10,11) //used as the head
    };

    vector<Interval> appearances_of_b{
        Interval(4,5),
        Interval(12,13),
    };
    //skipping several head appearances in order to find the closest one to the tail apperance
    vector<Interval> appearances_of_ab{
        Interval(2,5),
        Interval(10,13)
    };
    // several appearances are found
    Finding_sequences finding_sequences;
    vector<Interval> found_pairs = 
       finding_sequences.find_repeated_pairs(appearances_of_a, appearances_of_b);

    REQUIRE(found_pairs == appearances_of_ab);
}

TEST_CASE("all tail apperances are before head appearances") {
    vector<Interval> appearances_of_a{
        Interval(6,7),
        Interval(8,9),
        Interval(9,10)
    };

    vector<Interval> appearances_of_b{
        Interval(0,1),
        Interval(2,3),
        Interval(4,5)
    };
    // no apperarances are found
    Finding_sequences finding_sequences;
    vector<Interval> found_pairs = 
       finding_sequences.find_repeated_pairs(appearances_of_a, appearances_of_b);

    REQUIRE(found_pairs == vector<Interval>(0));
}

TEST_CASE("all head appearances are before tail apperances") {
    vector<Interval> appearances_of_a{
        Interval(0,1),
        Interval(2,3),
        Interval(4,5)
    };

    vector<Interval> appearances_of_b{
        Interval(6,7),
        Interval(8,9),
        Interval(10,11)
    };

    vector<Interval> appearances_of_ab{
        Interval(4,7)
    };
    // only one appearance is found
    Finding_sequences finding_sequences;
    vector<Interval> found_pairs = 
       finding_sequences.find_repeated_pairs(appearances_of_a, appearances_of_b);

    REQUIRE(found_pairs == appearances_of_ab);
}

TEST_CASE("no appearances of the head") {
    vector<Interval> appearances_of_a{};

    vector<Interval> appearances_of_b{
        Interval(6,7),
        Interval(8,9),
        Interval(9,10)
    };
    // no apperarances are found
    Finding_sequences finding_sequences;
    vector<Interval> found_pairs = 
       finding_sequences.find_repeated_pairs(appearances_of_a, appearances_of_b);

    REQUIRE(found_pairs.empty());
}


TEST_CASE("intervals are tightly overlapping") {
    vector<Interval> appearances_of_a{
        Interval(0,1),
        Interval(1,2),
        Interval(2,3),
        Interval(3,4),
        Interval(4,5),
        Interval(5,6)
    };

    vector<Interval> appearances_of_b{
        Interval(0,1),
        Interval(1,2),
        Interval(2,3),
        Interval(3,4),
        Interval(4,5),
        Interval(5,6)
    };
    vector<Interval> appearances_of_ab{
        Interval(0,3),
        Interval(1,4),
        Interval(2,5),
        Interval(3,6)
    };

    Finding_sequences finding_sequences;
    vector<Interval> found_pairs = 
       finding_sequences.find_repeated_pairs(appearances_of_a, appearances_of_b);

    REQUIRE(found_pairs == appearances_of_ab);
}
