#pragma once

#include "../Database/Database.h"

#include <iostream>
#include <memory>



namespace rvinowise::ai {

/**/
static class Finding_sequences {
    public:
    static std::vector<Interval> find_repeated_pairs(std::vector<Interval>, std::vector<Interval>);


};

}