using System.Collections.Generic;
using rvinowise.ai.patterns;
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

    void Awake() {
        
    }
    private void create_curved_line() {
        /* Contract.Requires(
            start is Component && end is Component,
            "GameObjects are required for visualization"
        );*/
        bezier.init_between_points(
            ((Component)start).transform,
            ((Component)end).transform,
            new Vector3(0, 4f),
            new Vector3(0, 2f)
        );

    }

   

   
}


}