using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.unity;

namespace rvinowise.ai.general {

public interface IFigure:
IHave_destructor
{

    string id { get; }

    IReadOnlyList<IFigure_appearance> all_appearances { get; }
    IReadOnlyList<IFigure_representation> get_representations();
    
    IReadOnlyList<IFigure_appearance> get_appearances_in_interval(
        BigInteger start, BigInteger end
    );

    IReadOnlyList<IFigure> as_lowlevel_sequence();

    void add_appearance(
        IFigure_appearance appearance
    );

    IReadOnlyList<IFigure_representation> get_representations();
}

}