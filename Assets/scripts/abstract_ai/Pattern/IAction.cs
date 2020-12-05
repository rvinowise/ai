
using System.Collections.Generic;
using rvinowise.ai.action;
using rvinowise.ai.patterns;


namespace rvinowise.ai.action {


public interface IAction {
    
    IPattern pattern{get;}
    type_t type {get;}
    IAction_group action_group{get;}
    IPattern_appearance pattern_appearance{get;}
    
}

public enum type_t {
    start = 0,
    end = 1,
}
}