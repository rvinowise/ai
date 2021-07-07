

using rvinowise.ai.patterns;
using rvinowise.unity.ai;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using UnityEngine;

namespace rvinowise.unity.ai.action {


public class Start_appearance: 
Action,
IAppearance_start
{
    public new Start_appearance put_into_moment(
        IAction_group in_action_group
    ) {
        return base.put_into_moment(
            in_action_group
        ) as Start_appearance;
    }

    
   
}
}