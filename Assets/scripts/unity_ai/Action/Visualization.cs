
using System;
using UnityEngine;
using TMPro;
using rvinowise.unity.extensions;

namespace rvinowise.unity.ai.action {


public partial class Action: 
MonoBehaviour,
IHave_destructor
{

    [SerializeField]
    private TextMeshPro lable;
    [HideInInspector]
    public Animator animator;
    
    void Awake() {
        animator = GetComponent<Animator>();
    }
    public void set_label(string in_text) {
        lable.text = in_text;
    }

    
    void OnMouseDown() {
        if (figure_appearance is Pattern_appearance appearance) {
            appearance.selected_with_subfigures =
                !appearance.selected_with_subfigures;
        }

    }
    
    public bool highlighted {
        set {
            _highlighted = value;
            animator.SetBool("selected", _highlighted);
            if (value) {
                transform.set_z(transform.parent.position.z - 1);
            } else {
                transform.set_z(transform.parent.position.z - 0.1f);
            }
        }
        get {return _highlighted;}
    }
    private bool _highlighted;
    
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
}
}