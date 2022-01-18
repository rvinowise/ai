using System.Numerics;
using rvinowise.ai.general;
using System.Collections.Generic;
using System;
using rvinowise.unity;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.extensions.pooling;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


namespace rvinowise.ai.unity {

public class Figure_appearance:
    MonoBehaviour,
    IFigure_appearance,
    IHave_destructor
{
    
    
    #region IFigure_appearance
    public IFigure figure{get; protected set;}
    public BigInteger start_moment 
        => start_appearance.action_group.moment;
    public BigInteger end_moment 
        => end_appearance.action_group.moment;

    #endregion IFigure_appearance

    
    public Appearance_start start_appearance;
    public Appearance_end end_appearance;



    void Awake() {
        pooled_object = GetComponent<Pooled_object>();
        start_appearance.figure_appearance = this;
        end_appearance.figure_appearance = this;
    }

    
    

    public virtual void destroy() {
        store_action_as_child(start_appearance);
        store_action_as_child(end_appearance);
        start_appearance.transform.parent = transform;
        end_appearance.transform.parent = transform;
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
            start_appearance.transform,
            end_appearance.transform,
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
        start_appearance.set_label(figure.id);
        end_appearance.set_label(figure.id);
    }

    #region debug
    IList<ISubfigure_appearance> subfigure_appearances 
        = new List<ISubfigure_appearance>();

    #endregion

    
}
}