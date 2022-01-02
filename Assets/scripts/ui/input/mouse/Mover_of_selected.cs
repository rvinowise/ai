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
    public Selection selection;

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

    void Update() {
       
        if (UnityEngine.Input.GetMouseButtonDown (0)) {    
            ISelectable selectable = Selection.instance.get_selectable_under_mouse();                        
            if (selectable != null) {
                is_moving = true;                       
            }
        }

        if (UnityEngine.Input.GetMouseButtonUp(0)) {
            is_moving = false;
        }
        
        if (is_moving) {
            Vector3 difference = mouse_world_position - old_mouse_position;
            update_position(difference);
        }
        old_mouse_position = mouse_world_position;
    }

    private void update_position(Vector3 difference) {
        foreach(ISelectable selectable in Selection.instance.selectables) {
            selectable.transform.position += difference;
        }
    }

}
}