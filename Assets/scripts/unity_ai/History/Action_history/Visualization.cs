/* visualises all the actions that were input into the system */
using System.Collections.Generic;
using rvinowise.unity.ai.action;
using UnityEngine;
using rvinowise.unity.extensions;

namespace rvinowise.unity.ai {
public partial class Action_history
{
    
    public Action_group action_group_prefab;
    public Pattern_appearance pattern_appearance_preafab;
    public Vector2 action_group_offset = new Vector2(2f,0f);
    public float lines_offset = -4f;
    
    public Transform carret;


    private void place_new_action_group(Action_group in_group) {
        in_group.transform.parent = transform;
        in_group.transform.position = carret.position;
        carret.Translate(action_group_offset);
    }

    public override void start_new_line() {
        carret.position = new Vector2(
            0,
            carret.position.y+lines_offset
        );
    }
    
    private void place_new_pattern_appearance(
        Pattern_appearance appearance
    ) {
        appearance.transform.parent = transform;
    }
    
}
}