#pragma once

#include <stdint.h>
#include <string>

#include "Interval/Interval.h"


namespace rvinowise::ai {

class Figure_appearance {
    Interval interval;

    public:
    Figure_appearance(Interval);
}; 

}