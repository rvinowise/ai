
using rvinowise.ai.action;
using rvinowise.ai.patterns;
using rvinowise.unity.ai.patterns;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;


namespace rvinowise.unity.ai.action {


public partial class Action: IAction {
    

    public IPattern pattern{get;private set;}
    public type_t type{get;private set;} = type_t.start;
    public IAction_group action_group{get;private set;}
    public IPattern_appearance pattern_appearance{get;private set;}
    /* public Action_history action_history {
        get { return action_group.action_history;  }
    } */

    [called_by_prefab]
    public Action get_for_fired_pattern(
        Pattern in_pattern,
        Action_group in_group
    ) {
        Action new_action = this.get_from_pool<Action>();

        new_action.init_for_fired_pattern(in_pattern);
        new_action.action_group = in_group;
        
        return new_action;
    }
    
    public void init_for_fired_pattern(Pattern in_pattern) {
        pattern = in_pattern;
        type = type_t.fired;
        set_label(in_pattern.id);
    }
   
}
}