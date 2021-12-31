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
using rvinowise.unity.geometry2d;
using UnityEngine.EventSystems;

namespace rvinowise.unity.ui.input.mouse {

public class Rectangle {
    public float left; 
    public float right;
    public float bottom;
    public float top;

    public Vector2 top_left => new Vector2(left, top);
    public Vector2 top_right => new Vector2(right, top);
    public Vector2 bottom_left => new Vector2(left, bottom);
    public Vector2 bottom_right => new Vector2(right, bottom);

    public void update_from_points(Vector2 start, Vector2 end) {
        if (start.x > end.x) {
            left = end.x;
            right = start.x;
        } else {
            left = start.x;
            right = end.x;
        }
        if (start.y > end.y) {
            top = start.y;
            bottom = end.y;
        } else {
            top = end.y;
            bottom = start.y;
        }
    }
}
public class Box_selector: MonoBehaviour {

    #region what can be selected
    public Action_history action_history;
    #endregion

    
    private Vector2 start_position;
    private Rectangle rect = new Rectangle();
    private bool is_selecting = false;

    public LineRenderer line_renderer;

    void Awake() {
        line_renderer = GetComponent<LineRenderer>();
        line_renderer.loop = true;
        line_renderer.positionCount = 4;
    }

    void Update() {
        Vector2 mouse_position = Input.instance.mouse_world_position;
        
        if (UnityEngine.Input.GetMouseButtonDown(0)) {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            }
            
            start_selection(mouse_position);
        }

        if (UnityEngine.Input.GetMouseButtonUp(0)) {
            end_selection();
        }
        
        if (is_selecting) {
            update_selection(
                start_position, 
                mouse_position
            );
            if (start_position.distance_to(mouse_position) > 5) {
                bool test = true;
            }
        }
    }

    private void start_selection(Vector2 mouse_position) {
        start_position = mouse_position;
        line_renderer.enabled = true;
        is_selecting = true;
        deselect_previous();
    }

    private void end_selection() {
        line_renderer.enabled = false;
        is_selecting = false;
    }

    void deselect_previous() {
        Selection.instance.deselect_all();
    }

    private void update_selection(
        Vector2 start, Vector2 end
    ) {
        deselect_previous();
        rect.update_from_points(start, end);
        update_selecting_box(rect);
        select_objects();
    }


    private void update_selecting_box(Rectangle rect) {
        line_renderer.SetPosition(0, rect.top_left);
        line_renderer.SetPosition(1, rect.top_right);
        line_renderer.SetPosition(2, rect.bottom_right);
        line_renderer.SetPosition(3, rect.bottom_left);
    }
    private void select_objects() {
        IReadOnlyList<IAction_group> action_groups 
        = get_all_action_groups();
        foreach(IAction_group group in action_groups) {
            if (
                (group is ISelectable selectable)&&
                (is_inside_selection(selectable))
            ) {
                Selection.instance.select(selectable);
                
            }
        }
    }

    private IReadOnlyList<IAction_group> get_all_action_groups() {
        return action_history.get_action_groups(
            0,
            action_history.last_moment
        );// as IReadOnlyList<Action_group>;
    }

    bool is_inside_selection(ISelectable entity) {
        Vector2 position = entity.transform.position;
        return (
            (position.x > rect.left)&&
            (position.x < rect.right)&&
            (position.y > rect.bottom)&&
            (position.y < rect.top)
        );
    }


}
}