using System.Numerics;
using rvinowise.ai.general;
using System.Collections.Generic;
using System;
using rvinowise.unity;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.extensions.pooling;
using rvinowise.unity.ui.input;
using rvinowise.unity.ui.input.mouse;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


namespace rvinowise.ai.unity {

public class Figure_appearance:
    MonoBehaviour,
    IFigure_appearance,
    IHave_destructor,
    ISelectable
{
    
    
    #region IFigure_appearance
    public IFigure figure{get; protected set;}
    public BigInteger start_moment 
        => appearance_start.action_group.moment;
    public BigInteger end_moment 
        => appearance_end.action_group.moment;

    #endregion IFigure_appearance

    
    public Appearance_start appearance_start;
    public Appearance_end appearance_end;



    void Awake() {
        pooled_object = GetComponent<Pooled_object>();
        appearance_start.figure_appearance = this;
        appearance_end.figure_appearance = this;
        bezier.enabled = false;
    }

    
    

    public virtual void destroy() {
        store_action_as_child(appearance_start);
        store_action_as_child(appearance_end);
        appearance_start.transform.parent = transform;
        appearance_end.transform.parent = transform;
        ((MonoBehaviour)this).destroy();
    }

    private void store_action_as_child(Action in_action) {
        in_action.action_group.remove_action(in_action);
        in_action.transform.parent = transform;
    }

    #region visualisation
    
    public Bezier bezier;
    private Action_history action_history;

    private Pooled_object pooled_object;


    public void create_curved_line() {
        bezier.init_between_points(
            appearance_start.transform,
            appearance_end.transform,
            new Vector3(0, 4f),
            new Vector3(0, 2f)
        );
        
    }
    #endregion
    
    [called_by_prefab]
    public Figure_appearance get_for_figure(IFigure figure) {
        Figure_appearance appearance = 
            this.get_from_pool<Figure_appearance>();

        appearance.init_for_figure(figure);
        
        return appearance;
    }

    private void init_for_figure(IFigure figure) {
        this.figure = figure;
        appearance_start.set_label(figure.id);
        appearance_end.set_label(figure.id);
    }
    
    #region ISelectable

    public new Collider collider { get; }
    public SpriteRenderer selection_sprite_renderer => null;
    public void accept_selection(Selector selector) {
        selector.select(this);
    }

    public void accept_deselection(Selector selector) {
        selector.deselect(this);
    }

    #endregion

    #region debug
    IList<ISubfigure_appearance> subfigure_appearances 
        = new List<ISubfigure_appearance>();

    #endregion

    
}
}