#pragma once

#include <stdint.h>
#include <string>

#include "../Interval/Interval.hpp"


namespace rvinowise::ai {

class Figure_appearance {
    Interval interval;

    public:
    Figure_appearance(Interval);
}; 

}