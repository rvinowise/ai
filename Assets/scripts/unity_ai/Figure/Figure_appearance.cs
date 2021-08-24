using System.Numerics;
using abstract_ai;


namespace rvinowise.unity.ai.Figure {

public class Figure_appearance:IFigure_appearance {
    
    public IFigure figure { get; }
    public IFigure place { get; }
    public BigInteger start_moment { get; }
    public BigInteger end_moment { get; }
}
}