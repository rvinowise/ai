using System.Numerics;
using rvinowise.ai.general;
using System.Collections.Generic;
using rvinowise.ai.ui.general;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.extensions.pooling;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


namespace rvinowise.ai.unity {

public class Figure_appearance:
    MonoBehaviour,
    IVisual_figure_appearance,
    IHave_destructor
{
    
    
    #region IVisual_figure_appearance

    public IVisual_action get_start() => start_action;
    public IVisual_action get_end() => end_action;
    
    
    #region IFigure_appearance
    public IFigure figure{get; private set;}
    public BigInteger start_moment 
        => start_action.action_group.moment;
    public BigInteger end_moment 
        => end_action.action_group.moment;

    IAction IFigure_appearance.get_start() => start_action;
    IAction IFigure_appearance.get_end() => end_action;
    


    #endregion IFigure_appearance
    #endregion IVisual_figure_appearance
    
    private IVisual_action start_action;
    private IVisual_action end_action;


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
            ((Action)start_action).transform,
            ((Action)end_action).transform,
            new Vector3(0, 4f),
            new Vector3(0, 2f)
        );
        
    }
    
    #region IAccept_selection

    public Selection_of_object selection { get; } = new Figure_appearance_selection();

    #endregion IAccept_selection
    
    #endregion visualisation
    
    
    
    

    #region debug
    IList<ISubfigure_appearance> subfigure_appearances 
        = new List<ISubfigure_appearance>();

    #endregion

    
}
}