#pragma once

#include "../Finding_sequences/Finding_sequences.h"

namespace rvinowise::ai {

/* because client code uses this library by invoking a C-style procedure, 
we have to have a global state of this whole library to store reusable stuff 
(e.g. db connections) */
class Main {

    private:
    
    inline static std::unique_ptr<Main> instance;

    public:
    Main();
    ~Main();

    static void init_module();
    static Main* get_instance();
   
    

};

}