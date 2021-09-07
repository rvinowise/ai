using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.patterns;

namespace abstract_ai {

public interface IFigure {

    string id { get; }
    
    string as_dot_graph();
    
    IReadOnlyList<IFigure_appearance> get_appearances_in_interval(
        BigInteger start, BigInteger end
    );
}

}