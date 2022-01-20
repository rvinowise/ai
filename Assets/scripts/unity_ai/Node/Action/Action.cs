
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.unity;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;
using rvinowise.unity.ui.input.mouse;
using TMPro;
using UnityEngine;

namespace rvinowise.ai.unity {


public class Action: 
MonoBehaviour,
IAction,
IHave_destructor,
ISelectable
{
    #region IAction
    public IFigure figure => figure_appearance.figure;
    public IFigure_appearance figure_appearance {
        get => figure_appearance_impl;
        set {
            figure_appearance_impl = value as Figure_appearance;
        }
    }

    #endregion IAction
    
    public IAction_group action_group{get;set;}

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
        ((MonoBehaviour)this).destroy_object();
    }
    #endregion

    #region ISelectable
    public void accept_selection(Selector selector) {
        selector.select(this);
    }
    public void accept_deselection(Selector selector) {
        selector.deselect(this);
    }
    
    public SpriteRenderer selection_sprite_renderer => sprite_renderer;
    [SerializeField] private SpriteRenderer sprite_renderer;
    public new Collider collider{get;set;}
    #endregion ISelectable
    
    #region debug

    [SerializeField] public Figure_appearance figure_appearance_impl;

    #endregion debug

    #endregion visualisation
}
}