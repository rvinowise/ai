#pragma once

#include "../Database/Database.h"

#include <iostream>
#include <memory>



namespace rvinowise::ai {

/**/
class Finding_sequences {
    
    public:
    Finding_sequences();
    ~Finding_sequences();

    std::vector<Interval> find_repeated_pairs(std::vector<Interval>, std::vector<Interval>);


};

}