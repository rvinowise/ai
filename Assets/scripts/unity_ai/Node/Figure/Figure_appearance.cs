using System.Numerics;
using rvinowise.ai.general;
using System.Collections.Generic;

namespace rvinowise.ai.unity {

public class Figure_appearance:IFigure_appearance {
    public string id;
    public IFigure figure { get; }
    public BigInteger start_moment { get; }
    public BigInteger end_moment { get; }

    #region debug
    IList<ISubfigure_appearance> subfigure_appearances 
        = new List<ISubfigure_appearance>();

    #endregion
}
}