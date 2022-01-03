
using System;
using UnityEngine;
using TMPro;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input.mouse;

namespace rvinowise.unity.ai.action {


public partial class Action: 
MonoBehaviour,
IHave_destructor,
ISelectable
{

    [SerializeField]
    private TextMeshPro lable;
    [HideInInspector]
    public Animator animator;
    
    void Awake() {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }
    public void set_label(string in_text) {
        lable.text = in_text;
    }
    
    public string as_dot_graph() {
        throw new NotImplementedException();
    }
    
    #region IHave_destructor
    
    public void destroy()
    {
        action_group.remove_action(this);
        ((MonoBehaviour)this).destroy();
    }
    #endregion

    #region ISelectable
    public bool selected {
        set {
            _selected = value;
            animator.SetBool("selected", _selected);
            // if (value) {
            //     transform.set_z(transform.parent.position.z - 1);
            // } else {
            //     transform.set_z(transform.parent.position.z - 0.1f);
            // }
            // if (figure_appearance is Pattern_appearance appearance) {
            //     appearance.selected_with_subfigures =
            //         !appearance.selected_with_subfigures;
            // }
        }
        get {return _selected;}
    }
    private bool _selected;
    //Collider ISelectable.collider => collider;
    public SpriteRenderer selection_sprite_renderer => sprite_renderer;
    [SerializeField]
    private SpriteRenderer sprite_renderer;
    public new Collider collider{get;set;}
    #endregion
}
}