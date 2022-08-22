#pragma once

#include <stdint.h>

namespace rvinowise::ai {

struct Interval {
    uint64_t head;
    uint64_t tail;
}; 

}