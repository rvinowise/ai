
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.unity;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;
using rvinowise.unity.ui.input.mouse;
using TMPro;
using UnityEngine;

namespace rvinowise.ai.simple {


public class Action: 
IAction
{
    public Action_type type{get;}

    public Action(
        Action_type in_type,
        IFigure_appearance in_appearance,
        IAction_group in_group
    ) {
        type = in_type;
        figure_appearance = in_appearance;
        action_group = in_group;
    }

    #region IAction
    public IFigure figure => figure_appearance.figure;
    public IFigure_appearance figure_appearance { get; }
    public IAction_group action_group{get;}

    #endregion IAction
    

}
}