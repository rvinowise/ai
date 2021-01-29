

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
    public new Start_appearance init_for_pattern_appearance(
        Pattern_appearance appearance,
        IAction_group in_action_group
    ) {
        return base.init_for_pattern_appearance(
            appearance,
            in_action_group
        ) as Start_appearance;
    }

    
   
}
}