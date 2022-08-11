using rvinowise.ai.ui.general;
using UnityEngine;
using Object = System.Object;


namespace rvinowise.ai.ui.unity {


public class Selection_of_rendered_object: 
    ai.ui.general.Selection_of_object 
{
    //Object selected_object { get; }
    public Selection_of_rendered_object(SpriteRenderer sprite_renderer) {
        this.sprite_renderer = sprite_renderer;
    }

    private readonly SpriteRenderer sprite_renderer;
    
    #region general.ISelection_of_object

    protected override void set_normal_state() {
        sprite_renderer.color = normal_color;
    }

    protected override void set_highlighted_state() {
        sprite_renderer.color = highlighted_color;
    }

    protected override void set_selected_state() {
        sprite_renderer.color = selected_color;
    }
    
    #endregion general.ISelection_of_object

}

}