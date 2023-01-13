#pragma once

#include <stdint.h>
#include <compare>
#include <ostream>


namespace rvinowise::ai {

using Moment =  int32_t;//uint64_t

struct Interval {
    public:
    Interval();
    Interval(Moment head, Moment tail);

    Moment head;
    Moment tail;
    Moment get_head() const {return head;};
    Moment get_tail() const {return tail;};

    //auto operator<=>(const Interval&) const = default;
    bool operator==(const Interval& o) const;

    friend std::ostream& operator<<(std::ostream& os, const Interval& interval);
   
}; 

}