
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.unity;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;
using rvinowise.unity.ui.input.mouse;
using TMPro;
using UnityEngine;

namespace rvinowise.ai.simple {


public struct Action: 
IAction
{
    #region IAction
    public IFigure figure => figure_appearance.figure;
    public IFigure_appearance figure_appearance { get; set; }

    #endregion IAction
    


}
}