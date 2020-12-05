using System.Collections;
using System.Collections.Generic;
using rvinowise.unity.ai.patterns;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.ai.action;
using rvinowise.ai.patterns;
using type_t = rvinowise.ai.action.type_t;
using UnityEngine;
using System.Numerics;

namespace rvinowise.unity.ai.action {

public partial class Action_group:IAction_group {
  
    private IList<IAction> actions = new List<IAction>();
    public IEnumerator<IAction> GetEnumerator() => actions.GetEnumerator();

    [HideInInspector]
    public Action_history action_history;
    [HideInInspector] public BigInteger moment{get; protected set;}
    
    [called_by_prefab]
    public Action_group get_for_selected_patterns_of(
        Pattern_storage pattern_storage
    ) {
        Action_group new_group = this.get_from_pool<Action_group>();
        new_group.init_for_selected_patterns_in(
            pattern_storage.known_patterns
        );
        return new_group;
    }
    public void init_for_selected_patterns_in(
        ICollection<Pattern> in_patterns
    ) {
        int patterns_count = 0;
        foreach (Pattern pattern in in_patterns) {
            if (pattern.selected) {
                Action new_action = action_prefab.get_for_fired_pattern(
                    pattern,
                    this
                );
                place_next_action(new_action);
                pattern.add_appearance();
                new_action.animator.SetTrigger("fire");
                patterns_count++;
            }
        }
        extend_to_accomodate_children(patterns_count);
        
    }


    public bool has_action(IPattern pattern, type_t type) {
        foreach (var action in actions) {
            if (
                (action.pattern == pattern)&&
                (action.type == type)
                ) {
                return true;
            }
        }
        
        return false;
    }
    
}
}