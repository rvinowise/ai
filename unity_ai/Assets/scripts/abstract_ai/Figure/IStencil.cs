using System.Collections.Generic;


namespace rvinowise.ai.general {

public interface IStencil:
IFigure_representation {
    IReadOnlyList<ISubfigure> get_outputs();
    IReadOnlyList<ISubfigure> get_inputs();


}

}