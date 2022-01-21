using System.Collections;
using System.Collections.Generic;
using rvinowise.ai.unity;
using rvinowise.unity.extensions;
using UnityEngine;
using TMPro;
using rvinowise.unity.ui.input.mouse;
using rvinowise.unity.ui.input;
using rvinowise.ai.general;

namespace rvinowise.ai.unity {

public partial class Action_group:
MonoBehaviour,
ISelectable
{
    public Vector3 action_offset = new Vector3(0,1, 0);
    //public GameObject body;
    public Mood_label mood_label;
    public TextMeshPro moment_label;
    [SerializeField]
    SpriteRenderer actions_sprite_renderer;

    [SerializeField]
    Transform actions_attachment;
 
    private void place_next_action(Action in_action) {
        in_action.transform.parent = this.actions_attachment;
        in_action.transform.localPosition = action_offset * (actions.Count-1);
    }

    public void update_shape_accomodating_children() {
        actions_sprite_renderer.size = 
            new Vector2(
                actions_sprite_renderer.size.x,
                action_offset.y * (actions.Count) + 0.5f
            );

    }

    #region ISelectable
    public void accept_selection(Selector selector) {
        selector.select(this);
    }
    public void accept_deselection(Selector selector) {
        selector.deselect(this);
    }
    public new Collider collider => null;
  

    [SerializeField]
    public SpriteRenderer selection_sprite_renderer => actions_sprite_renderer;
 
    #endregion
    
}
}