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
        => start_action.action_group.moment;
    public BigInteger end_moment 
        => end_action.action_group.moment;

    public IAction get_start() => start_action;
    public IAction get_end() => end_action;

    #endregion IFigure_appearance

    
    public Action start_action;
    public Action end_action;



    void Awake() {
        pooled_object = GetComponent<Pooled_object>();
        start_action.figure_appearance = this;
        end_action.figure_appearance = this;
        //bezier.gameObject.SetActive(true); //test
    }
    
    

    public virtual void destroy() {
        start_action.destroy();
        end_action.destroy();
        this.destroy_object();
    }



    #region visualisation
    
    public Bezier bezier;
    private Action_history action_history;

    private Pooled_object pooled_object;


    public void create_curved_line() {
        bezier.init_between_points(
            start_action.transform,
            end_action.transform,
            new Vector3(0, 4f),
            new Vector3(0, 2f)
        );
        
    }
    #endregion
    
    [called_by_prefab]
    public Figure_appearance get_for_figure(IFigure figure) {
        Figure_appearance appearance = 
            this.provide_new<Figure_appearance>();

        appearance.init_for_figure(figure);
        
        return appearance;
    }

    private void init_for_figure(IFigure figure) {
        this.figure = figure;

        start_action.set_label(figure.id);
        end_action.set_label(figure.id);
    }
    
    #region ISelectable

    public new Collider collider { get; }
    public Renderer selection_sprite_renderer => null;
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