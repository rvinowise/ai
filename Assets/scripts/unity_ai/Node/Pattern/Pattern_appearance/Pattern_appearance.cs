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
public class Pattern_appearance: 
Repetition_appearance,
IPattern_appearance,
IHave_destructor
{

    
    #region debug
    public IFigure_appearance first_half;
    public IFigure_appearance second_half;
    #endregion debug


    public bool selected_with_subfigures {
        get {return _selected_with_subfigures;}
        set {
            if (first_half is Pattern_appearance first_appearance) {
                first_appearance.selected_with_subfigures = value;
            }
            if (second_half is Pattern_appearance second_appearance) {
                second_appearance.selected_with_subfigures = value;
            }
            _selected_with_subfigures = value;
            selected = value;
        }

    }
    private bool _selected_with_subfigures;

    public override void destroy() {
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

}
}