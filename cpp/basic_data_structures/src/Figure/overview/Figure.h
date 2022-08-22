#pragma once

#include <stdint.h>
#include <string>
#include <vector>

#include "../Appearance/Appearance.h"
#include "../Edge/Edge.h"


namespace rvinowise::ai {

class Figure {

    std::string id;
    std::vector<Figure_appearance> appearances;
    std::vector<Edge> edges;

    public:
    Figure(string id);
};

}