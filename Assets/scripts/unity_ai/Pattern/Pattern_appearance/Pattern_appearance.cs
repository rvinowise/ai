using rvinowise.rvi.contracts;
using rvinowise.unity.ai.action;

using rvinowise.ai.patterns;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.extensions.pooling;
using UnityEngine;

namespace rvinowise.unity.ai.patterns {

[RequireComponent(typeof(Pooled_object))]
public partial class Pattern_appearance: IPattern_appearance {

    #region IPattern_appearance
    public IStart_appearance start{get; protected set;}
    public IEnd_appearance end{get; protected set;}
    IPattern IPattern_appearance.pattern => pattern;

    #endregion

    public Pattern pattern{get; protected set;}
    
    public Start_appearance start_appearance_prefab;
    public End_appearance end_appearance_prefab;

    [called_by_prefab]
    public Pattern_appearance get_for_interval(
        Pattern in_pattern,
        IAction_group start_group,
        IAction_group end_group
    ) {
        Contract.Requires(
            start_group != end_group,
            ""
        );
        Pattern_appearance appearance = 
            this.get_from_pool<Pattern_appearance>();
        
        appearance.pattern = in_pattern;
        
        appearance.start = 
            start_appearance_prefab.
            get_from_pool<Start_appearance>().
            init_for_pattern_appearance(appearance);
        
        appearance.end = 
            end_appearance_prefab.
            get_from_pool<End_appearance>().
            init_for_pattern_appearance(appearance);

        start_group.add_action(appearance.start);
        end_group.add_action(appearance.end);
        
        appearance.create_curved_line();
        
        return appearance;
    }
}
}