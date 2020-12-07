
using System.Collections.Generic;
using rvinowise.ai.patterns;


namespace rvinowise.ai.patterns {


public interface IAction {
    
    IPattern pattern{get;}
    IAction_group action_group{get;}
    IPattern_appearance pattern_appearance{get;}
    
}

}