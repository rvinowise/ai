
using System;
using UnityEngine;
using TMPro;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input.mouse;
using rvinowise.unity;

namespace rvinowise.ai.unity {


public partial class Action: 
MonoBehaviour,
IHave_destructor,
ISelectable
{
    #region visualisation
    
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
        }
        get {return _selected;}
    }
    private bool _selected;
    public SpriteRenderer selection_sprite_renderer => sprite_renderer;
    [SerializeField]
    private SpriteRenderer sprite_renderer;
    public new Collider collider{get;set;}
    #endregion ISelectable
    
    #endregion visualisation
}
}