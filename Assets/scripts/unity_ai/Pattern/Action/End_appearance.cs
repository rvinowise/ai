

using rvinowise.ai.patterns;
using rvinowise.unity.ai.patterns;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;


namespace rvinowise.unity.ai.action {


public class End_appearance: 
Action,
IEnd_appearance
{

    public new End_appearance init_for_pattern_appearance(
        Pattern_appearance appearance
    ) {
        return base.init_for_pattern_appearance(appearance) as End_appearance;
    }
   
}
}