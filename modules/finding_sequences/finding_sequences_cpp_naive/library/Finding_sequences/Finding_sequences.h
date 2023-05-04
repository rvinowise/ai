#pragma once


#include <iostream>
#include <memory>
#include <vector>

#include "Interval/Interval.h"


namespace rvinowise::ai {

class Finding_sequences {
    public:
    static std::vector<Interval> find_repeated_pairs(std::vector<Interval>, std::vector<Interval>);


};

}