using System.Collections;
using System.Collections.Generic;
using rvinowise.ai.unity;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.ai.general;
using UnityEngine;
using System.Numerics;
using rvinowise.rvi.contracts;
using rvinowise.unity.ui.input.mouse;

namespace rvinowise.ai.simple {

public class Action_group:
IAction_group
 {
  
    public IEnumerator<IAction> GetEnumerator() => 
        actions.GetEnumerator();

    public BigInteger moment{
        get {
            return _moment;
        }
        private set {
            _moment = value;
        }
    }

    public float mood {
        get;
        private set;
    }

    public void init_mood(float value) {
        mood = value;
    }

    private BigInteger _moment;
    private readonly IList<IAction> actions = new List<IAction>();
    

    public Action_group(
        BigInteger moment,
        float mood = 0f
    ) {
        this.moment = moment;
        this.mood = mood;
    }
    

    public void add_action(IAction in_action) {
        actions.Add(in_action);
    }

    public void remove_action(IAction in_action) {
        actions.Remove(in_action);
    }


    public bool has_action<TAction>(IFigure figure) where TAction: IAction {
        foreach (var action in actions) {
            if (
                (action.figure == figure)&&
                (action is TAction)
                ) {
                return true;
            }
        }
        
        return false;
    }

  

 
}
}