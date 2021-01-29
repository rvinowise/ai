using rvinowise.rvi.contracts;
using rvinowise.unity.ai.action;

using rvinowise.ai.patterns;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.extensions.pooling;
using UnityEngine;
using System.Numerics;

namespace rvinowise.unity.ai {

[RequireComponent(typeof(Pooled_object))]
public partial class Pattern_appearance: 
IPattern_appearance,
IHave_destructor
{

    #region IPattern_appearance
    public IAppearance_start start => start_appearance;
    public IAppearance_end end => end_appearance;
    IPattern IPattern_appearance.pattern => pattern;

    public BigInteger start_moment => start.action_group.moment;
    public BigInteger end_moment => end.action_group.moment;

    #endregion

    public Pattern pattern{get; protected set;}
    
    public Start_appearance start_appearance;
    public End_appearance end_appearance;

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
        
        appearance.start_appearance.
            init_for_pattern_appearance(appearance, start_group);
        appearance.end_appearance.
            init_for_pattern_appearance(appearance, end_group);

        appearance.create_curved_line();
        
        return appearance;
    }

    public bool is_entirely_before(IPattern_appearance appearance) {
        return end_moment < appearance.start_moment;
    }

    public virtual void destroy() {
        if (start is IHave_destructor destructable_start) {
            destructable_start.destroy();
        }
        if (end is IHave_destructor destructable_end) {
            destructable_end.destroy();
        }
        ((MonoBehaviour)this).destroy();
    }
}
}