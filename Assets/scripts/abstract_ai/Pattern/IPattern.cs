

using System.Collections.Generic;
using System.Numerics;

namespace rvinowise.ai.general {
public interface IPattern:
IFigure
{

    IReadOnlyList<IFigure> subfigures { get; }
    

    
    

    IReadOnlyList<IFigure> as_lowlevel_sequence();

}

}