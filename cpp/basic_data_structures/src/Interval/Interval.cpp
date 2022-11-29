#include "Interval.h"
#include <iostream>

using namespace std;

namespace rvinowise::ai {

Interval::Interval()
{
}

Interval::Interval(uint64_t head, uint64_t tail):
head{head}, tail{tail} 
{
}

bool Interval::operator==(const Interval& o) const {
    return 
        (this->head==o.head) 
        &&
        (this->tail==o.tail);
}

std::ostream& operator<<(std::ostream& os, const Interval& interval) {
    return os << "{" << interval.head << ", " << interval.tail << "}";
}

}