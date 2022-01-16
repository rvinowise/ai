
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


}
}