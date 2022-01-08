

using System.Collections.Generic;
using System.Numerics;

namespace rvinowise.ai.general {
public interface IPattern:
IFigure
{

    IReadOnlyList<IFigure> subfigures { get; }
    

    IPattern_appearance create_appearance(
        BigInteger start,
        BigInteger end
    );
    IPattern_appearance create_appearance(
        IFigure_appearance first_half,
        IFigure_appearance second_half
    );

    IReadOnlyList<IFigure> as_lowlevel_sequence();

}

}