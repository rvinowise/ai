using rvinowise.rvi.contracts;
using rvinowise.ai.unity;

using rvinowise.ai.general;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.extensions.pooling;
using UnityEngine;
using System.Numerics;
using rvinowise.unity;

namespace rvinowise.ai.unity {

[RequireComponent(typeof(Pooled_object))]
public partial class Pattern_appearance: 
IPattern_appearance,
IHave_destructor
{

    #region IPattern_appearance

    public BigInteger start_moment 
        => start_appearance.action_group.moment;
    public BigInteger end_moment 
        => end_appearance.action_group.moment;

    #endregion

    public IPattern pattern{get; protected set;}
    
    public Appearance_start start_appearance;
    public Appearance_end end_appearance;

    #region debug
    public IFigure_appearance first_half;
    public IFigure_appearance second_half;
    #endregion debug

    [called_by_prefab]
    public Pattern_appearance get_for_interval(
        Pattern in_pattern,
        BigInteger start,
        BigInteger end
    ) {
        Contract.Requires(
            start < end,
            "should have a positive time interval"
        );
        Pattern_appearance appearance = 
            this.get_from_pool<Pattern_appearance>();
        
        appearance.pattern = in_pattern;
        
        appearance.start_appearance.
            put_into_moment(start);
        appearance.end_appearance.
            put_into_moment(end);

        appearance.create_curved_line();
        
        return appearance;
    }

    public Pattern_appearance get_for_subfigures(
        Pattern in_pattern, 
        IFigure_appearance in_first_half,
        IFigure_appearance in_second_half        
    ) {
        Contract.Assert(
            in_pattern.id.Length > 1, 
            "Pattern consisting of subfigures should have a longer name"
        );

        Pattern_appearance appearance = get_for_interval(
            in_pattern,
            in_first_half.start_moment,
            in_second_half.end_moment

        );
        appearance.first_half = in_first_half;
        appearance.second_half = in_second_half;
        return appearance;
    }

    void Awake() {
        pooled_object = GetComponent<Pooled_object>();
        start_appearance.figure_appearance = this;
        end_appearance.figure_appearance = this;
    }

    
    

    public virtual void destroy() {
        store_action_as_child(start_appearance);
        store_action_as_child(end_appearance);
        start_appearance.transform.parent = transform;
        end_appearance.transform.parent = transform;
        first_half = null;
        second_half = null;
        ((MonoBehaviour)this).destroy();
    }

    private void store_action_as_child(Action in_action) {
        in_action.action_group.remove_action(in_action);
        in_action.transform.parent = transform;
    }

    #region IFigure_appearance

    public IFigure figure => pattern;
    public IFigure place { get; }
    #endregion
}
}