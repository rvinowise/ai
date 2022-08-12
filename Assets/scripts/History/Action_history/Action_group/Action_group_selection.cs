

using System.Linq;
using rvinowise.ai.general;
using rvinowise.ai.ui.general;
using UnityEditor;
using UnityEngine;


namespace rvinowise.ai.unity {

public class Action_group_selection:
ai.ui.unity.Selection_of_rendered_object 
{
    private IAction_group action_group;
    public Action_group_selection(SpriteRenderer sprite_renderer) : base(sprite_renderer) { }

    protected override void set_normal_state() {
        base.set_normal_state();
        action_group.get_actions().ToList().ForEach(action => 
            action.ui.selection.set_state(Selection_state.Normal)
        );
    }

    protected override void set_highlighted_state() {
        base.set_highlighted_state();
        figure.ui.button.selection.set_state(Selection_state.Highlighted);
    }

    protected override void set_selected_state() {
        base.set_selected_state();
        figure.button.selection.set_state(Selection_state.Highlighted);
        figure.get_appearances().ToList().ForEach(
            appearance => appearance.selection.set_state(Selection_state.Highlighted)
        );
    }
    


}

}