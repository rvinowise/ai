
using System.Numerics;
using rvinowise.ai.general;
using UnityEngine;

namespace rvinowise.ai.unity {


public partial class Action: 
IAction
{
    #region IAction
    public IFigure figure{get;private set;}
    public IFigure_appearance figure_appearance{get; internal set;}
    
    #endregion IAction
    
    public IAction_group action_group{get;set;}

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

    
   
}
}