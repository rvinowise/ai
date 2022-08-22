#pragma once

#include "../Subfigure/Subfigure.h"

#include <stdint.h>
#include <string>

using string = std::string;

namespace rvinowise::ai {

class Edge {
    std::string id;
    Subfigure head;
    Subfigure tail;

    public:
    Edge(std::string id, Subfigure head, Subfigure tail);
}; 

}