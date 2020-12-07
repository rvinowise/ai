

using rvinowise.ai.patterns;
using rvinowise.unity.ai.patterns;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;


namespace rvinowise.unity.ai.action {


public class Start_appearance: 
Action,
IStart_appearance
{

    public new Start_appearance init_for_pattern_appearance(
        Pattern_appearance appearance
    ) {
        return base.init_for_pattern_appearance(appearance) as Start_appearance;
    }
   
}
}