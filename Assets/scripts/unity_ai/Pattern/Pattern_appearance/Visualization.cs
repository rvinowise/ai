using System.Collections.Generic;
using rvinowise.unity.ai.action;
using UnityEngine;
using Action = rvinowise.unity.ai.action.Action;

namespace rvinowise.unity.ai.patterns {
public partial class Pattern_appearance: MonoBehaviour {
    public Bezier bezier;
    public Bezier bezier_prefab;
    private Action_history action_history;

    void Awake() {
        
    }
    private void create_curved_line() {
        /* Contract.Requires(
            start is Component && end is Component,
            "GameObjects are required for visualization"
        );*/
        bezier = bezier_prefab.get_between_points(
            ((Component)start).transform,
            ((Component)end).transform,
            new Vector3(0, 4f),
            new Vector3(0, 2f)
        );

    }

   

   
}


}