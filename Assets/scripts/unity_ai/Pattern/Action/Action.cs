
using System.Numerics;
using abstract_ai;
using rvinowise.ai.patterns;
using rvinowise.unity.ai;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using UnityEngine;

namespace rvinowise.unity.ai.action {


public partial class Action: 
IAction
{
    #region IAction interface
    public IFigure figure{get;private set;}
    public IFigure_appearance figure_appearance{get; internal set;}
    
    #endregion //IAction
    
    public IAction_group action_group{get;private set;}

    void Start() {
        figure = figure_appearance.figure;
        if (figure is IPattern pattern) {
            set_label(pattern.id);
        }
        else {
            set_label("f");
        }
    }

    

    public void put_into_moment(
        BigInteger in_moment
    ) {
        Action_group in_action_group = Action_history.instance.
            get_action_group_at_moment(in_moment);
        in_action_group.add_action(this);
        action_group = in_action_group;
    }

    public void destroy()
    {
        action_group.remove_action(this);
        ((MonoBehaviour)this).destroy();
    }
   
}
}