using rvinowise.rvi.contracts;
using rvinowise.unity.ai.action;
using rvinowise.ai.action;
using rvinowise.ai.patterns;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.extensions.pooling;
using UnityEngine;
using rvinowise.ai.patterns;

namespace rvinowise.unity.ai.patterns {

[RequireComponent(typeof(Pooled_object))]
public partial class Pattern_appearance: IPattern_appearance {
    public IAction start{get; protected set;}
    public IAction end{get; protected set;}
    

    [called_by_prefab]
    public Pattern_appearance get_for_fired_pattern(
        IAction fireing_action
    ) {
        Pattern_appearance appearance = 
            this.get_from_pool<Pattern_appearance>();
        appearance.start = fireing_action;
        appearance.end = fireing_action;
        return appearance;
   
    }

    [called_by_prefab]
    public Pattern_appearance get_for_interval(
        IAction start,
        IAction end
    ) {
        //Contract.Requires(start.action_history == end.action_history);
        Contract.Requires(
            start != end,
          "no interval's span exist - use another constructor"
        );
        Pattern_appearance appearance = 
            this.get_from_pool<Pattern_appearance>();
        appearance.start = start;
        appearance.end = end;
        appearance.create_curved_line();
        
        return appearance;
    }
}
}