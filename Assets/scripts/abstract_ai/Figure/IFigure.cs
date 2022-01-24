using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.unity;

namespace rvinowise.ai.general {

public interface IFigure:
IHave_destructor
{

    string id { get; }

    IReadOnlyList<IFigure_appearance> get_appearances();
    
    IReadOnlyList<IFigure_representation> get_representations();
    IFigure_representation create_representation();
    
    IReadOnlyList<IFigure_appearance> get_appearances_in_interval(
        BigInteger start, BigInteger end
    );

    void add_appearance(
        IFigure_appearance appearance
    );
    
    IReadOnlyList<IFigure> as_lowlevel_sequence();
    

}

}