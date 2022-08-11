

using System.Linq;
using rvinowise.ai.general;
using rvinowise.ai.ui.general;
using UnityEditor;
using UnityEngine;


namespace rvinowise.ai.unity {

public class Figure_selection:
ai.ui.unity.Selection_of_rendered_object 
{
    private IVisual_figure figure;
    private Selection_state state = Selection_state.Normal;
    public Figure_selection(SpriteRenderer sprite_renderer) : base(sprite_renderer) { }

    protected override void set_normal_state() {
        base.set_normal_state();
        figure.button.selection.set_state(Selection_state.Normal);
    }

    protected override void set_highlighted_state() {
        base.set_highlighted_state();
        figure.button.selection.set_state(Selection_state.Highlighted);
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