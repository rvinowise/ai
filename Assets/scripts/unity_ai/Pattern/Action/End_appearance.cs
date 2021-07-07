

using rvinowise.ai.patterns;
using rvinowise.unity.ai;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;


namespace rvinowise.unity.ai.action {


public class End_appearance: 
Action,
IAppearance_end
{

    public new End_appearance put_into_moment(
        IAction_group in_action_group
    ) {
        return base.put_into_moment(
            in_action_group
        ) as End_appearance;
    }
   
}
}