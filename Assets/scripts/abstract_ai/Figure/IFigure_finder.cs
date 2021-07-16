using System.Collections.Generic;

namespace abstract_ai {

public interface IFigure_finder {

    IReadOnlyList<IFigure_appearance> find_common_subfigures_in(
        IReadOnlyList<IFigure> in_places
    );

}

}