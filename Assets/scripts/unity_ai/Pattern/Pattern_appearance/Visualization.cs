using System.Collections.Generic;
using rvinowise.ai.patterns;
using rvinowise.unity.extensions.pooling;
using UnityEngine;
using Action = rvinowise.unity.ai.action.Action;

namespace rvinowise.unity.ai {
public partial class Pattern_appearance: 
MonoBehaviour,
IPattern_appearance,
IHave_destructor
{
    public Bezier bezier;
    private Action_history action_history;

    private Pooled_object pooled_object;

    public bool selected {
        get {return _selected;}
        set {
            _selected = value;
            start_appearance.highlighted = value;
            end_appearance.highlighted = value;
            bezier.gameObject.SetActive(value);
        }
    }
    private bool _selected;
    private void create_curved_line() {
        bezier.init_between_points(
            ((Component)start).transform,
            ((Component)end).transform,
            new Vector3(0, 4f),
            new Vector3(0, 2f)
        );
        
    }

}


}