#pragma once

#include <stdint.h>
#include <compare>
#include <ostream>


namespace rvinowise::ai {

struct Interval {
    public:
    Interval();
    Interval(uint64_t head, uint64_t tail);

    uint64_t head;
    uint64_t tail;
    uint64_t get_head() const {return head;};
    uint64_t get_tail() const {return tail;};

    //auto operator<=>(const Interval&) const = default;
    bool operator==(const Interval& o) const;

    friend std::ostream& operator<<(std::ostream& os, const Interval& interval);
   
}; 

}