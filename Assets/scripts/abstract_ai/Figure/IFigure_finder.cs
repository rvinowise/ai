using System.Collections.Generic;

namespace rvinowise.ai.general {

public interface IFigure_finder {

    IReadOnlyList<IFigure_appearance> find_common_subfigures_in(
        IReadOnlyList<IFigure> in_places
    );

}

}