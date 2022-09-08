#include "Database.h"

#include "Figure/Figure_appearance/Figure_appearance.h"

#include <iostream>
#include <format>
#include <memory>

#include "spdlog/spdlog.h"

using namespace std;

namespace rvinowise::ai {

Database::Database(std::string connection_string) {
    soci::register_factory_postgresql();
    session = new soci::session(connection_string);
}

Database::~Database() {
    session->close();
}

std::vector<Interval> Database::fetch_appearances(const std::string figure) {
    vector<Interval> result;
    result.reserve(1000);
    try {
        soci::rowset<soci::row> rows = (session->prepare << 
            R"(
            select Head, Tail 
            from Figure_appearance
            where Figure = :figure
            )", soci::use(figure)
        );
        
        spdlog::info("fetched figure {}", figure);
        for (
            soci::rowset<soci::row>::const_iterator it = rows.begin(); 
            it != rows.end(); 
            ++it
        ) {
            soci::row const& row = *it;
            spdlog::info(std::format(
                "head:{}; tail:{}",
                row.get<int64_t>(0),
                row.get<int64_t>(1)    
            ));
            result.push_back(
                Interval(
                    row.get<int64_t>(0),
                    row.get<int64_t>(1)
                )
            );
        }
    } catch (const std::exception& e)  {
        std::cout << e.what();
    }
    spdlog::info(
        "fetching appearances allocated {} memory with {} intervals",
        sizeof(std::vector<Interval>) + sizeof(Interval) * result.size(),
        result.size()
    );

    return result;
}

void Database::commit_appearances(
    const string figure,
    const std::vector<Interval> appearances
) {

}

}