using System.Collections.Generic;
using rvinowise.ai.general;
using rvinowise.unity.extensions.pooling;
using UnityEngine;
using Action = rvinowise.ai.unity.Action;

namespace rvinowise.ai.unity {
public partial class Repetition_appearance: 
MonoBehaviour
{
    public Bezier bezier;
    private Action_history action_history;

    private Pooled_object pooled_object;

    public bool selected {
        get {return _selected;}
        set {
            _selected = value;
            start_appearance.selected = value;
            end_appearance.selected = value;
            bezier.gameObject.SetActive(value);
        }
    }
    private bool _selected;
    

    

    public void create_curved_line() {
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