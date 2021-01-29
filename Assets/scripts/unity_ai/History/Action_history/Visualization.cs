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
    
    private Vector2 carret = new Vector2(0,0);


    private void place_new_action_group(Action_group in_group) {
        in_group.transform.parent = transform;
        in_group.transform.localPosition = carret;
        carret += action_group_offset;
    }
    
    private void place_new_pattern_appearance(
        Pattern_appearance appearance
    ) {
        appearance.transform.parent = transform;
    }
    
}
}