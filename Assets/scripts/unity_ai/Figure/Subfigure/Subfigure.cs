
using System;
using System.Collections.Generic;
using abstract_ai;

namespace rvinowise.unity.ai.figure {

public class Subfigure: ISubfigure {

    public Subfigure(IFigure figure) {
        this.figure = figure;
    }
    public IFigure parent { get; private set; }
    public IFigure figure {get; private set; }

    public IReadOnlyList<ISubfigure> next { get{
        return _next.AsReadOnly();
    }}
    
    public IReadOnlyList<ISubfigure> previous { get{
        return _previous.AsReadOnly();
    }}

    private List<ISubfigure> _next =   new List<ISubfigure>();
    private List<ISubfigure> _previous =   new List<ISubfigure>();

    public string id;

    public String get_name() {
        return String.Format("{0}({1})", figure.id, id);
    }
    
    #region building
    public void connext_to_next(ISubfigure next_subfigure) {
        append_next(next_subfigure);
        next_subfigure.append_previous(this);
    }
    public void append_next(ISubfigure subfigure) {
        _next.Add(subfigure);
    }
    public void append_previous(ISubfigure subfigure) {
        _previous.Add(subfigure);
    }

    #endregion
}
}