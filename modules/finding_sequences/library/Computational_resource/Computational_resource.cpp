#include "Computational_resource.h"

#include <iostream>
#include <format>
#include <memory>

using namespace std;

namespace rvinowise::ai {

Computaional_resource::Computaional_resource() {}
Computaional_resource::~Computaional_resource(){}

int Computaional_resource::get_available_memory() {
    return 1000;
}

int Computaional_resource::get_vailable_cpu_threads() {
    return 4;
}

}