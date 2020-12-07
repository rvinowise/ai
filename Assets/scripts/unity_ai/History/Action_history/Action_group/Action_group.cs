using System.Collections;
using System.Collections.Generic;
using rvinowise.unity.ai.patterns;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.ai.patterns;
using UnityEngine;
using System.Numerics;

namespace rvinowise.unity.ai.action {

public partial class Action_group:IAction_group {
  
    public IEnumerator<IAction> GetEnumerator() => actions.GetEnumerator();

    [HideInInspector] public BigInteger moment{get; protected set;}
    private IList<IAction> actions = new List<IAction>();
    
    [called_by_prefab]
    public Action_group get_for_moment(
        BigInteger moment
    ) {
        Action_group new_group = this.get_from_pool<Action_group>();
        new_group.moment = moment;
        return new_group;
    }
    

    public void add_action(IAction in_action) {
        actions.Add(in_action);
        if (in_action is Action action) {
            place_next_action(action);
            action.animator.SetTrigger("fire");
            
        }
    }


    public bool has_action<TAction>(IPattern pattern) where TAction: IAction {
        foreach (var action in actions) {
            if (
                (action.pattern == pattern)&&
                (action is TAction)
                ) {
                return true;
            }
        }
        
        return false;
    }

 
}
}