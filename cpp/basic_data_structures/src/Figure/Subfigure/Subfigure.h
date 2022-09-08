#pragma once

#include <stdint.h>
#include <string>
#include <vector>

#include "../Figure_appearance/Figure_appearance.h"


namespace rvinowise::ai {

class Subfigure {

    std::string id;
    //Figure parent;
    //Figure referenced;
    public:
    Subfigure(std::string id);
};

}