
using System.Collections.Generic;

namespace rvinowise.ai.general {

public interface ISubfigure {
    string id {get; set; }
    IFigure_representation parent { get; }
    IFigure referenced_figure {get; }
    IReadOnlyList<ISubfigure> next { get; }
    IReadOnlyList<ISubfigure> previous { get; }

    #region building
    void connext_to_next(ISubfigure next_subfigure);
    void disconnect_from_next(ISubfigure next);
    void append_previous(ISubfigure subfigure);
    void remove_previous(ISubfigure subfigure);
    #endregion
}

}