using UnityEngine;
using Object = System.Object;


namespace rvinowise.ai.ui.general {

public enum Selection_state {
    Normal,
    Selected,
    Highlighted
}

public abstract class Selection_of_object {
    //Object selected_object { get; }

    protected Color selected_color = new(0,1,0);
    protected Color normal_color = new(1,1,1);
    protected Color highlighted_color = new(0.9f,1,0.9f);

    public void set_state(Selection_state state) {
        this.state = state;
        switch (state) {
            case Selection_state.Normal:
                set_normal_state();
                break;
            case Selection_state.Highlighted:
                set_highlighted_state();
                break;
            case Selection_state.Selected:
                set_selected_state();
                break;
        }
    }
    public Selection_state get_state() {
        return state;
    }

    protected virtual void set_normal_state() { }
    protected virtual void set_highlighted_state() { }
    protected virtual void set_selected_state() { }



    private Selection_state state = Selection_state.Normal;

    
}
}