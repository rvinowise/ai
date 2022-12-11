#pragma once

#include "Interval/Interval.h"

namespace rvinowise::ai {

class Interval_in_sequence {
    
    private:
    
    bool _exists;
    size_t _index;
    Interval _interval;

    public:

    Interval_in_sequence() ;
    Interval_in_sequence(size_t index, Interval interval);
    static Interval_in_sequence non_existent();
    bool exists() const {return _exists;}
    size_t index() const {return _index;}
    Interval interval() const {return _interval;}
    uint64_t head() {return _interval.head;}
    uint64_t tail() {return _interval.tail;}
};

}