
using System.Collections.Generic;


namespace rvinowise.ai.general {


public interface IAction {
    
    IFigure figure{get;}
    IFigure_appearance figure_appearance{get;}
    
    IAction_group action_group{get;}
}

}