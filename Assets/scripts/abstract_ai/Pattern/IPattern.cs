

using System.Collections.Generic;
using System.Numerics;
using abstract_ai;

namespace rvinowise.ai.patterns {
public interface IPattern:
IFigure
{

    string id {
        get;
    }
    
    public IFigure first_half { get; }
    public IFigure second_half { get; } 

    IPattern_appearance create_appearance(
        BigInteger start,
        BigInteger end
    );
    IPattern_appearance create_appearance(
        IFigure_appearance first_half,
        IFigure_appearance second_half
    );

    

}

}