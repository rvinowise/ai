using System.Collections.Generic;
using System.Numerics;

namespace abstract_ai {
public interface IFigure_appearance {
    
    IFigure figure { get; }
    
    BigInteger start_moment { get; }
    BigInteger end_moment { get; }


}
}