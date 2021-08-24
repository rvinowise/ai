using System.Collections.Generic;
using System.Numerics;
using abstract_ai;

namespace rvinowise.unity.ai.Figure {

public class Figure: IFigure {
    
    public IReadOnlyList<ISubfigure> first_subfigures { get; private set; } = new List<ISubfigure>();

    #region IFigure

    public string id { get; }

    public string as_dot_graph() {
        throw new System.NotImplementedException();
    }

    public IReadOnlyList<IFigure_appearance> get_appearances(IFigure in_where) {
        throw new System.NotImplementedException();
    }

    public IReadOnlyList<IFigure_appearance> get_appearances_in_interval(BigInteger start, BigInteger end) {
        throw new System.NotImplementedException();
    }
    #endregion IFigure
    
    
}
}