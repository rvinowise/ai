using System.Collections.Generic;
using rvinowise.ai.patterns;
using rvinowise.unity.extensions.pooling;
using UnityEngine;
using Action = rvinowise.unity.ai.action.Action;

namespace rvinowise.unity.ai {
public partial class Pattern_appearance: 
MonoBehaviour
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
    private bool _selected_with_subfigures;

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
    
    private void create_curved_line() {
        bezier.init_between_points(
            ((Component)start_appearance).transform,
            ((Component)end_appearance).transform,
            new Vector3(0, 4f),
            new Vector3(0, 2f)
        );
        
    }
    
    void Start() {
        selected = false;
    }

}


}