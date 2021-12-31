using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using rvinowise;
using rvinowise.rvi.contracts;
using Input = rvinowise.unity.ui.input.Input;
using rvinowise.unity.ai;
using rvinowise.unity.ai.action;
using Action = rvinowise.unity.ai.action.Action;
using rvinowise.ai.patterns;
using rvinowise.unity.geometry2d;
using UnityEngine.Assertions;
using rvinowise.unity.ui.input.mouse;

namespace rvinowise.unity.ui.input {

public class Selected {
    
}

public class Selection : MonoBehaviour {

    #region what can be selected

    public Action_history action_history;
    

    #endregion

    public HashSet<IAction_group> action_groups = new HashSet<IAction_group>();
    public IReadOnlyList<IAction_group> sorted_action_groups {
        get {
            return action_groups.OrderBy(
                group => group.moment  
            ).ToList();
        }
    }
    public HashSet<ISelectable> selectables = new HashSet<ISelectable>();

    public static Selection instance;
    
    void Awake() {
        Contract.Assert(instance == null, "singleton");
        instance = this;
    }

    public void select(Action_group action_group) {
        action_groups.Add(action_group);
        if (action_group is Action_group unity_group) {
            foreach (IAction action in unity_group.actions) {
                if (action is Action unity_action) {
                    unity_action.selected = true;
                }
            }
        }
    }
    public void select(ISelectable selectable) {
        selectables.Add(selectable);
        selectable.selected = true;
    }

    public void deselect(IAction_group action_group) {
        deselect_actions_of_group(action_group);
        action_groups.Remove(action_group);
    }
    public void deselect(ISelectable selectable) {
        selectables.Remove(selectable);
        selectable.selected = false;
    }

    private void deselect_actions_of_group(IAction_group action_group) {
        if (action_group is Action_group unity_group) {
            foreach (IAction action in unity_group.actions) {
                if (action is Action unity_action) {
                    unity_action.selected = false;
                }
            }
        }
    }

    public void deselect_all() {
        foreach (IAction_group action_group in action_groups) {
            deselect_actions_of_group(action_group);
        }
        action_groups.Clear();
        selectables.Clear();
    }


    private IReadOnlyList<IAction_group> get_all_action_groups() {
        return action_history.get_action_groups(
            0,
            action_history.last_moment
        );// as IReadOnlyList<Action_group>;
    }



}
}