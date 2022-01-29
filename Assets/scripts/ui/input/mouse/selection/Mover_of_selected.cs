using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rvinowise;
using rvinowise.rvi.contracts;


namespace rvinowise.unity.ui.input.mouse {

public class Mover_of_selected: MonoBehaviour {

    public static Mover_of_selected instance;
    [SerializeField]
    public Selector selector;

    private bool is_moving;
    private Vector3 old_mouse_position;
    public HashSet<Transform> moved_things = new HashSet<Transform>();


    public void add_moved_thing(Component thing) {
        moved_things.Add(thing.transform);
    }

    public void remove_all_moved_things() {
        moved_things.Clear();
    }
    void Awake() {
        init_singleton();
    }
    private void init_singleton() {
        Contract.Requires(instance == null, "singleton should be initialised once");
        instance = this;
    }

    

    public bool moved_since_last_click;
    public bool moved_in_this_frame;
    public void update() {
       
        if (Input.GetMouseButtonDown (0)) {    
            start_moving();
        }

        if (Input.GetMouseButtonUp(0)) {
            stop_moving();
        }
        
        if (is_moving) {
            Vector3 difference = (Vector3)Unity_input.instance.mouse_world_position - old_mouse_position;
            update_position(difference);
        }
        old_mouse_position = Unity_input.instance.mouse_world_position;
    }

    private void start_moving() {
        is_moving = true;
    }
    private void stop_moving() {
        moved_since_last_click = false;
        is_moving = false;
    }

    private void update_position(Vector3 difference) {
        if (difference.magnitude > float.Epsilon) {
            moved_since_last_click = true;
            moved_in_this_frame = true;
            foreach(Transform thing in moved_things) {
                thing.position += difference;
            }
        }
        else {
            moved_in_this_frame = false;
        }

    }

}
}