#include "../include/finding_sequences.h"

#include <iostream>
#include <stdio.h>
#include <soci/soci.h>
#include <soci/postgresql/soci-postgresql.h>

using string = std::string;
//using cout = std::cout;

extern "C"
{

DLLEXPORT void find_repeated_pairs(
    const char* connection_string
) {
    soci::register_factory_postgresql();
    try {
        std::cout << "start";
        //soci::session sql("postgres://postgres: @127.0.0.1:5432/ai");
        soci::session sql(connection_string);
        std::cout << "session created";
        soci::rowset<soci::row> rows = (sql.prepare << "select figure, head, tail from Figure_appearance");
        std::cout << "before cycle";
        for (soci::rowset<soci::row>::const_iterator it = rows.begin(); it != rows.end(); ++it) {
            //std::cout << *it << '\n';
            soci::row const& row = *it;
            printf(
                "figure:%s; head:%d; tail:%d\n",
                row.get<string>(0),
                row.get<int64_t>(1),
                row.get<int64_t>(2)
            );
        }
    } catch (const std::exception& e)  {
        std::cout << e.what();
    }
}


}