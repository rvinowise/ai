
using System.Collections.Generic;

namespace rvinowise.ai.general {

public interface ISubfigure {
    string id {get;set;}
    IFigure_representation parent { get; set; }
    IFigure referenced_figure {get; }
    IReadOnlyList<ISubfigure> next { get; }
    IReadOnlyList<ISubfigure> previous { get; }

    #region building
    public void connext_to_next(ISubfigure next_subfigure);
    public void disconnect_from_next(ISubfigure next_subfigure);
    public void append_previous(ISubfigure subfigure);
    public void remove_previous(ISubfigure subfigure);
    #endregion
}

}