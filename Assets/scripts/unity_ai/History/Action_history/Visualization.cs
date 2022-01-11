/* visualises all the actions that were input into the system */
using System.Collections.Generic;
using rvinowise.ai.unity;
using UnityEngine;
using rvinowise.unity.extensions;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;

namespace rvinowise.ai.unity {
public partial class Action_history
{
    
    public Action_group action_group_prefab;
    public Pattern_appearance pattern_appearance_prefab;
    public Figure_appearance figure_appearance_prefab;
    public Vector2 action_group_offset = new Vector2(2f,0f);
    public float lines_offset = -4f;
    
    public Transform carret;


    protected void Awake() {
        Contract.Assert(instance==null, "singleton");
        instance = this;
    }
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
    
    private void place_new_figure_appearance(
        IFigure_appearance in_appearance
    ) {
        if (in_appearance is Pattern_appearance appearance) {
            appearance.transform.parent = transform;
        }
    }
    
}
}