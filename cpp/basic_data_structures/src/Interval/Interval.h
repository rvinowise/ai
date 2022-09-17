#pragma once

#include <stdint.h>
#include <compare>


namespace rvinowise::ai {

struct Interval {
    public:
    Interval();
    Interval(uint64_t head, uint64_t tail);

    uint64_t head;
    uint64_t tail;

    auto operator<=>(const Interval&) const = default;
}; 

}