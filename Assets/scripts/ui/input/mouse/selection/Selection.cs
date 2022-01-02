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
using rvinowise.unity.ai.figure;

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
    public HashSet<Subfigure> subfigures = new HashSet<Subfigure>();
    public HashSet<ISelectable> selectables = new HashSet<ISelectable>();

    public static Selection instance;

    public Color selected_color = new Color(1,0,0);
    public Color normal_color = new Color(1,1,1);
    private Vector3 get_mouse_position_from_top() {
        return new Vector3(
            rvinowise.unity.ui.input.Input.instance.mouse_world_position.x,
            rvinowise.unity.ui.input.Input.instance.mouse_world_position.y,
            -100
        );
    }
    private Vector3 mouse_world_position {
        get {
            return new Vector3(
                rvinowise.unity.ui.input.Input.instance.mouse_world_position.x,
                rvinowise.unity.ui.input.Input.instance.mouse_world_position.y,
                0
            );
        }
    }

    
    void Awake() {
        Contract.Assert(instance == null, "singleton");
        instance = this;
    }

    public void select(Subfigure subfigure) {
        subfigures.Add(subfigure);
    }
    public void select(Action_group action_group) {
        action_groups.Add(action_group);
        
    }
    public void select(ISelectable selectable) {
        selectables.Add(selectable);
        selectable.selected = true;
        selectable.sprite_renderer.color = selected_color;
    }

    public void deselect(IAction_group action_group) {
        action_groups.Remove(action_group);
    }
    public void deselect(ISelectable selectable) {
        selectables.Remove(selectable);
        selectable.selected = false;
        selectable.sprite_renderer.color = normal_color;
    }


    public void deselect_all() {
        foreach(ISelectable selectable in selectables) {
            selectable.selected = false;
        }
        action_groups.Clear();
        subfigures.Clear();
        selectables.Clear();
    }


    private IReadOnlyList<IAction_group> get_all_action_groups() {
        return action_history.get_action_groups(
            0,
            action_history.last_moment
        );// as IReadOnlyList<Action_group>;
    }

    void Update() {
        
        if (UnityEngine.Input.GetMouseButtonDown (0)) {    
            ISelectable selectable = get_selectable_under_mouse();                        
            if (selectable != null) {
                if (!selectable.selected) {
                    select(selectable);  
                } else {
                    deselect(selectable);
                }
            }
        }
    }    

    public ISelectable get_selectable_under_mouse() {
        Ray ray = new Ray(get_mouse_position_from_top(), Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            if(
                hit.transform.GetComponent<ISelectable>() 
                is ISelectable selectable
            ) { 
                return selectable;
            }
        }
        return null;
    }
}
}