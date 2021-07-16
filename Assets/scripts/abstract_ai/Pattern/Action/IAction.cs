
using System.Collections.Generic;
using abstract_ai;
using rvinowise.ai.patterns;


namespace rvinowise.ai.patterns {


public interface IAction {
    
    IFigure figure{get;}
    IFigure_appearance figure_appearance{get;}
    
    // IFigure figure { get; }
    // IFigure_appearance figure_appearance { get; }
}

}