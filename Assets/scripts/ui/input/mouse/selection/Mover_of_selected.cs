using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rvinowise;
using rvinowise.rvi.contracts;
using Input = rvinowise.unity.ui.input.Input;


namespace rvinowise.unity.ui.input.mouse {

public class Mover_of_selected: MonoBehaviour {

    public static Mover_of_selected instance;
    [SerializeField]
    public Selector selector;

    private bool is_moving;
    private Vector3 old_mouse_position;

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

    public void Awake() {
        init_singleton();
    }
    private void init_singleton() {
        Contract.Requires(instance == null, "singleton should be initialised once");
        instance = this;
    }

    

    public bool moved_since_last_click;
    public void update() {
       
        if (UnityEngine.Input.GetMouseButtonDown (0)) {    
            if (selector.last_click_target != null) {
                start_moving();
                Debug.Log("start_moving");           
            }
        }

        if (UnityEngine.Input.GetMouseButtonUp(0)) {
            stop_moving();
            Debug.Log("stop_moving");
        }
        
        if (is_moving) {
            Vector3 difference = mouse_world_position - old_mouse_position;
            update_position(difference);
        }
        old_mouse_position = mouse_world_position;
    }
    public void start_moving() {
        is_moving = true;
    }
    private void stop_moving() {
        moved_since_last_click = false;
        is_moving = false;
    }

    private void update_position(Vector3 difference) {
        if (difference.magnitude > float.Epsilon) {
            moved_since_last_click = true;
            //Debug.Log("moved_since_last_click = true");
            foreach(ISelectable selectable in selector.selectables) {
                selectable.transform.position += difference;
            }
        }
        
    }

}
}