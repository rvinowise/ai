#pragma once

#include "../Database/Database.h"
#include "../Finding_sequences/Finding_sequences.h"

namespace rvinowise::ai {

/* because client code uses this library by invoking a C-style procedure, 
we have to have a global state of this whole library to store reusable stuff 
(e.g. db connections) */
class Main {

    private:
    
    inline static std::unique_ptr<Main> instance;
    Database database;

    public:
    Main(const std::string connection_string);
    ~Main();

    static void init_module(const std::string connection_string);
    static Main* get_instance();
    std::vector<Interval> find_repeated_pairs_in_database(
        std::string head, std::string tail
    );
    // std::vector<Interval> find_repeated_pairs(
    //     std::vector<Interval>, std::vector<Interval>
    // );
    

};

}