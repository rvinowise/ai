
using System.Collections.Generic;
using abstract_ai;

namespace rvinowise.unity.ai.figure {

public interface ISubfigure {

    IFigure parent { get; }
    IFigure figure {get; }
    IReadOnlyList<ISubfigure> next { get; }
    IReadOnlyList<ISubfigure> previous { get; }

    #region building
    public void connext_to_next(ISubfigure next_subfigure);
    public void append_next(ISubfigure subfigure) ;
    public void append_previous(ISubfigure subfigure);
    #endregion
}

}