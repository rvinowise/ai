
#include "Interval.h"
#include <iostream>

using namespace std;

namespace rvinowise::ai {

Interval::Interval()
{
    cout << "interval is default constructed";
}

Interval::Interval(uint64_t head, uint64_t tail):
head{head}, tail{tail} 
{
    cout << "interval is constructed";
}

}