using System.Collections;
using System.Collections.Generic;
using rvinowise.unity.ai;
using rvinowise.unity.extensions;
using UnityEngine;
using TMPro;
using rvinowise.unity.ui.input.mouse;
using rvinowise.unity.ui.input;
using rvinowise.ai.patterns;

namespace rvinowise.unity.ai.action {

public partial class Action_group:
MonoBehaviour,
ISelectable
{
    public Vector2 action_offset = new Vector2(0,2);
    //public GameObject body;
    public Mood_label mood_label;
    public TextMeshPro moment_label;

    void Awake() {
        sprite_renderer = GetComponent<SpriteRenderer>();
    }
    private void place_next_action(Action in_action) {
        in_action.transform.parent = this.transform;
        in_action.transform.localPosition = 
            action_offset * (actions.Count-1);
    }

    public void extend_to_accomodate_children() {
        sprite_renderer.size += action_offset * (actions.Count-1);
        
    }

    #region ISelectable
    public new Collider collider => null;
    public bool selected {
        set {
            _selected = value;
            // if (value) {
            //     sprite_renderer.color = new Color(1,0,0);
            //     //select_actions();
            // } else {
            //     sprite_renderer.color = new Color(1,1,1);
            //     //deselect_actions();
            // }
        }
        get => _selected;
    }
    private bool _selected;

    private void select_actions() {
        foreach (Action action in actions) {
            action.selected = true;
        }
    }
    private void deselect_actions() {
        foreach (Action action in actions) {
            action.selected = false;
        }
    }
    [SerializeField]
    public SpriteRenderer sprite_renderer{get; private set;}
 
    #endregion
    
}
}