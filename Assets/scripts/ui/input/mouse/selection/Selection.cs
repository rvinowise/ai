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
using UnityEngine.EventSystems;

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
    public Mover_of_selected mover_of_selected;
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
        if (selectable.selection_sprite_renderer !=null) {
            selectable.selection_sprite_renderer.color = selected_color;
        }
        if (selectable is Action_group action_group) {
            action_groups.Add(action_group);
        }
    }

    public void deselect(IAction_group action_group) {
        action_groups.Remove(action_group);
    }
    public void deselect(ISelectable selectable) {
        selectables.Remove(selectable);
        mark_object_as_deselected(selectable);
    }
    private void mark_object_as_deselected(ISelectable selectable) {
        selectable.selected = false;
        if (selectable.selection_sprite_renderer!=null) {
            selectable.selection_sprite_renderer.color = normal_color;
        }
    }


    public void deselect_all() {
        foreach(ISelectable selectable in selectables) {
            mark_object_as_deselected(selectable);
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

    public ISelectable last_click_target;
    bool last_click_selected;
    void Update() {
        
        if (UnityEngine.Input.GetMouseButtonDown (0)) {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            } 
            last_click_target = get_selectable_under_mouse();                        
            if (last_click_target != null) { // clicked on an object
                if (!last_click_target.selected) { //the object is for selection by this click
                    select(last_click_target);
                    last_click_selected = true;
                    Debug.Log("select for "+last_click_target);
                } else { // the object is for moving (already was selected)
                    last_click_selected = false;
                    Debug.Log("click on selected "+last_click_target);
                }
                //mover_of_selected.start_moving_selected_objects();
            } else { // clicked on the emptiness
                deselect_all();
            }
        }
        //release button
        if (UnityEngine.Input.GetMouseButtonUp (0)) {
            if (
                (!last_click_selected)&&
                (!mover_of_selected.moved_since_last_click)
            ) {
                Debug.Log("release, last_click_selected==true for "+last_click_target);
                ISelectable target_of_release = get_selectable_under_mouse();
                //deselect on release. initial click is used to move objects.
                if (same_object_received_click_and_release(
                    target_of_release, last_click_target
                )) {
                    deselect(target_of_release);
                    Debug.Log("deselect for "+last_click_target);
                }
            }
        } 

        mover_of_selected.update();

        bool same_object_received_click_and_release(
            ISelectable target_of_release, ISelectable target_of_last_click
        ) {
            if (target_of_release == null) {
                return false;
            }
            return target_of_release == target_of_last_click;
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