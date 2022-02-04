using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.unity;

namespace rvinowise.ai.general {

public interface IStencil:
IFigure_representation {
    IReadOnlyList<ISubfigure> get_outputs();
    IReadOnlyList<ISubfigure> get_inputs();


}

}