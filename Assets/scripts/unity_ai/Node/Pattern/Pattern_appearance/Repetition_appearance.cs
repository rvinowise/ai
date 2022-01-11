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
public partial class Repetition_appearance: 
IPattern_appearance,
IHave_destructor
{

    #region IPattern_appearance

    public BigInteger start_moment 
        => start_appearance.action_group.moment;
    public BigInteger end_moment 
        => end_appearance.action_group.moment;

    #endregion IPattern_appearance

    public IFigure figure{get; protected set;}
    
    public Appearance_start start_appearance;
    public Appearance_end end_appearance;

    [called_by_prefab]
    public Pattern_appearance get_for_pattern(
        IFigure in_pattern
    ) {
        Pattern_appearance appearance = 
            this.get_from_pool<Pattern_appearance>();
        
        appearance.figure = in_pattern;
        
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
        ((MonoBehaviour)this).destroy();
    }

    private void store_action_as_child(Action in_action) {
        in_action.action_group.remove_action(in_action);
        in_action.transform.parent = transform;
    }


}
}