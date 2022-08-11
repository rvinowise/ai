

using System.Linq;
using rvinowise.ai.general;
using rvinowise.ai.ui.general;
using UnityEditor;
using UnityEngine;


namespace rvinowise.ai.unity {

public class Figure_appearance_selection:
ai.ui.general.Selection_of_object 
{
    private IVisual_figure_appearance figure_appearance;


    public Figure_appearance_selection() : base() { }
    protected override void set_normal_state() {
        base.set_normal_state();
        figure_appearance.get_start().selection.set_state(Selection_state.Normal);
        figure_appearance.get_end().selection.set_state(Selection_state.Normal);
    }
    protected override void set_highlighted_state() {
        base.set_highlighted_state();
        figure_appearance.get_start().selection.set_state(Selection_state.Highlighted);
        figure_appearance.get_end().selection.set_state(Selection_state.Highlighted);
    }
    protected override void set_selected_state() {
        base.set_selected_state();
        figure_appearance.get_start().selection.set_state(Selection_state.Selected);
        figure_appearance.get_end().selection.set_state(Selection_state.Selected);
    }


    
}

}