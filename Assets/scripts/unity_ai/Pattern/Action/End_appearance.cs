

using rvinowise.ai.patterns;
using rvinowise.unity.ai;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;


namespace rvinowise.unity.ai.action {


public class End_appearance: 
Action,
IAppearance_end
{

    public new End_appearance init_for_pattern_appearance(
        Pattern_appearance appearance,
        IAction_group in_action_group
    ) {
        return base.init_for_pattern_appearance(
            appearance,
            in_action_group
        ) as End_appearance;
    }
   
}
}