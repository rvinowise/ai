using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rvinowise;
using rvinowise.rvi.contracts;
using Input = rvinowise.unity.ui.input.Input;
using rvinowise.unity.ai;
using rvinowise.unity.ai.action;
using Action = rvinowise.unity.ai.action.Action;
using rvinowise.ai.patterns;

namespace rvinowise.unity.ui.input.mouse {

public class Box_selector: MonoBehaviour {

    #region what can be selected
    public Action_history action_history;
    #endregion
    private Vector2 start_position;

    private Vector2 bottom_left_position;
    private Vector2 top_right_position;

    void Update() {
        Vector2 mouse_position = Input.instance.mouse_world_position;

        if (UnityEngine.Input.GetMouseButtonDown(0)) {
            start_position = mouse_position;
            deselect_previous();
        }

        if (UnityEngine.Input.GetMouseButtonUp(0)) {

        }

        if (UnityEngine.Input.GetMouseButton(0)) {
            update_selection(
                start_position, 
                mouse_position
            );
        }
    }

    void deselect_previous() {

    }

    private void update_selection(
        Vector2 start, Vector2 end
    ) {
        update_positions_for_comparing(start, end);
        update_selecting_box(start, end);
        select_objects();
    }

    private void update_positions_for_comparing(
        Vector2 start, Vector2 end
    ) {
        float left; float right; float bottom; float top;
        if (start.x > end.x) {
            left = start.x;
            right = end.x;
        } else {
            left = end.x;
            right = start.x;
        }
        if (start.y > end.y) {
            bottom = start.y;
            top = end.y;
        } else {
            bottom = end.y;
            top = start.y;
        }

        bottom_left_position = new Vector2(left, bottom);
        top_right_position = new Vector2(right, top);
    }

    private void update_selecting_box(Vector2 start, Vector2 end) {

    }
    private void select_objects() {
        IReadOnlyList<IAction_group> action_groups 
        = get_all_action_groups();
        foreach(IAction_group group in action_groups) {
            if (
                (group is Action_group unity_group)&&
                (is_inside_selection(unity_group))
            ) {
                foreach(IAction action in unity_group.actions) {
                    if (action is Action unity_action) {
                        unity_action.selected = true;
                    }
                }
            }
        }
    }

    private IReadOnlyList<IAction_group> get_all_action_groups() {
        return action_history.get_action_groups(
            0,
            action_history.last_moment
        );// as IReadOnlyList<Action_group>;
    }

    bool is_inside_selection(MonoBehaviour entity) {
        Vector2 position = entity.transform.position;
        return (
            (position.x > bottom_left_position.x)&&
            (position.x < top_right_position.x)&&
            (position.y > bottom_left_position.y)&&
            (position.y < top_right_position.y)
        );
    }


}
}