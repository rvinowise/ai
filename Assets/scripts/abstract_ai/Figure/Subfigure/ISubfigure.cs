
using System.Collections.Generic;

namespace rvinowise.ai.general {

public interface ISubfigure {
    string id {get;}
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