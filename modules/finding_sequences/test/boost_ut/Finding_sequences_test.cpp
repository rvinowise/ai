#ifdef NO
#include "../../Finding_sequences.h"


#include <boost/ut.hpp>
#include <vector>


using namespace std;
using namespace rvinowise::ai;

int main() {

    using namespace boost::ut;

    "a steady sequence of appearances of the head figure"_test = [] {
        vector<Interval> appearances_of_a {
            Interval(8,9),
            Interval(10,11),
            Interval(12,13),
            Interval(14,15)
        };
        "tail figure appeared at the same moments as the head figure"_test = [&] {
            vector<Interval> appearances_of_b{
                Interval(8,9),
                Interval(10,11),
                Interval(12,13),
                Interval(14,15)
            };

            then("several repeater pairs are found") = [] {
                vector<Interval> appearances_of_ab{
                    Interval(0,3),
                    Interval(4,7)
                };

                Finding_sequences finding_sequences;
                vector<Interval> found_pairs =
                    finding_sequences.find_repeated_pairs(appearances_of_a, appearances_of_b);

                expect(equal(found_pairs, appearances_of_ab));
            }
        }

        "tail figure appeared only before the head figure"_test = [&] {
            vector<Interval> appearances_of_b{
                Interval(0,1),
                Interval(2,3),
                Interval(4,5),
                Interval(6,7),
            };

            Finding_sequences finding_sequences;
            vector<Interval> found_pairs =
                finding_sequences.find_repeated_pairs(appearances_of_a, appearances_of_b);

            expect(empty(found_pairs));
        }

        

        
    };

}
    
    
#endif