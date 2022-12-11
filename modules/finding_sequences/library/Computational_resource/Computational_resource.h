#pragma once

#include <iostream>
#include <format>
#include <memory>



namespace rvinowise::ai {

/* determine the amount of memory and cpu threads available for this module */
class Computaional_resource {

    public:
    Computaional_resource();
    ~Computaional_resource();
    int get_available_memory();
    int get_vailable_cpu_threads();

    private:

};

}