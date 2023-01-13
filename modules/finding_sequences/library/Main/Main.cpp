#include "Main.h"

#include <iostream>
#include <memory>


using namespace std;

namespace rvinowise::ai {

Main::Main()
{

}

Main::~Main() {
    
}

void Main::init_module() {
    instance = std::make_unique<Main>();
}

Main* Main::get_instance() {
    if (instance == nullptr) {
        throw new exception(
            "initialization of the Main object should precede getting its instance"
        );
    }
    return instance.get();
}




}