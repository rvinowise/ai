#pragma once

#include <stdint.h>
#include <string>
#include <vector>

#include "Figure/Figure_appearance/Figure_appearance.h"
#include "Figure/Edge/Edge.h"


namespace rvinowise::ai {

class Figure {

    std::string id;
    std::vector<Figure_appearance> appearances;
    std::vector<Edge> edges;

    public:
    Figure(string id);
};

}