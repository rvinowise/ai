
using System.Collections.Generic;
using abstract_ai;

namespace rvinowise.unity.ai.figure {

public class Output_subfigure: ISubfigure {
    public IFigure parent { get; private set; }

    public IReadOnlyList<ISubfigure> next { get; } = new List<ISubfigure>();
    public IReadOnlyList<ISubfigure> previous { get; } = new List<ISubfigure>();
}
}