#include "Main.h"

#include <iostream>
#include <format>
#include <memory>

#include <boost/contract.hpp>

using namespace std;

namespace rvinowise::ai {

Main::Main(const std::string connection_string):
database(connection_string)
{

}

Main::~Main() {
    
}

void Main::init_module(const std::string connection_string) {
    instance = std::make_unique<Main>(connection_string);
}

Main* Main::get_instance() {
    if (instance == nullptr) {
        throw new exception(
            "initialization of the Main object should precede getting its instance"
        );
    }
    return instance.get();
}


vector<Interval> Main::find_repeated_pairs(
        string head, string tail
) {
    auto head_appearances = database.fetch_appearances(head);
    auto tail_appearances = database.fetch_appearances(tail);
    return finding_sequences.find_repeated_pairs(head_appearances, tail_appearances);
}




}