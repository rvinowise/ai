using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.general;

namespace rvinowise.ai.general {

public interface IFigure {

    string id { get; }
    
    string as_dot_graph();
    
    IReadOnlyList<IFigure_appearance> get_appearances_in_interval(
        BigInteger start, BigInteger end
    );

    IReadOnlyList<IFigure> as_lowlevel_sequence();

    void add_appearance(
        IFigure_appearance appearance
    );
}

}