#pragma once


#include <iostream>
#include <format>
#include <memory>

#include <soci/soci.h>
#include <soci/postgresql/soci-postgresql.h>


#include "Interval/Interval.h"


namespace rvinowise::ai {

class Database {

    private:
    soci::session* session;

    public:
    Database(std::string connection_string);
    ~Database();
    std::vector<rvinowise::ai::Interval> fetch_appearances(const std::string figure);
    void commit_appearances(
        const std::string figure, 
        const std::vector<rvinowise::ai::Interval> appearances
    );
};

}