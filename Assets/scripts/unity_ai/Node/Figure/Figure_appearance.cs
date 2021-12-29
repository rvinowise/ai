using System.Numerics;
using abstract_ai;
using System.Collections.Generic;

namespace rvinowise.unity.ai.figure {

public class Figure_appearance:IFigure_appearance {
    
    public IFigure figure { get; }
    public BigInteger start_moment { get; }
    public BigInteger end_moment { get; }

    #region debug
    IList<ISubfigure_appearance> subfigure_appearances 
        = new List<ISubfigure_appearance>();

    #endregion
}
}