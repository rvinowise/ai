

using rvinowise.ai.patterns;
using rvinowise.unity.ai;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using UnityEngine;

namespace rvinowise.unity.ai.action {


public partial class Action: 
IAction
{

    public IPattern pattern{get;private set;}
    public IAction_group action_group{get;private set;}
    public IPattern_appearance pattern_appearance{get;private set;}

  
    
    public virtual Action init_for_pattern_appearance(
        Pattern_appearance in_appearance,
        IAction_group in_action_group
    ) {
        pattern = in_appearance.pattern;
        pattern_appearance = in_appearance;
        set_label(pattern.id);
        in_action_group.add_action(this);
        action_group = in_action_group;
        return this;
    }

    public void destroy()
    {
        action_group.remove_action(this);
        ((MonoBehaviour)this).destroy();
    }
   
}
}