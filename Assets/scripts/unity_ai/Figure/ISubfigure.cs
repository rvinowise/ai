
using System.Collections.Generic;
using abstract_ai;

namespace rvinowise.unity.ai.Figure {

public interface ISubfigure {

    IFigure parent { get; }
    IFigure figure { get; }
    IReadOnlyList<ISubfigure> next { get; }
    IReadOnlyList<ISubfigure> previous { get; }

}

}