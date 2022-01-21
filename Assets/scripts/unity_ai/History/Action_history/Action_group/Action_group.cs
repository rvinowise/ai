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

namespace rvinowise.ai.unity {

public partial class Action_group:
IAction_group,
ISelectable
 {
  
    public IEnumerator<IAction> GetEnumerator() => 
        actions.GetEnumerator();

    [HideInInspector] public BigInteger moment{
        get {
            return _moment;
        }
        protected set {
            _moment = value;
            moment_label.SetText(value.ToString());
        }
    }

    public float mood {
        get;
        private set;
    }

    public void init_mood(float value) {
        mood = value;
        mood_label.set_mood(value);
    }

    private BigInteger _moment;
    public IList<IAction> actions = new List<IAction>();
    

    [called_by_prefab]
    public Action_group get_for_moment(
        BigInteger moment,
        float mood = 0f
    ) {
        Action_group new_group = this.provide_new<Action_group>();
        new_group.moment = moment;
        new_group.mood = mood;
        return new_group;
    }
    

    public void add_action(IAction in_action) {
        actions.Add(in_action);
        if (in_action is Action action) {
            place_next_action(action);
        }
        update_shape_accomodating_children();
    }

    

    public void remove_action(IAction in_action) {
        actions.Remove(in_action);
        update_shape_accomodating_children();
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