using System;
using rvinowise.rvi.contracts;
using UnityEngine;


namespace rvinowise.unity.ui.input {
public class Unity_input: MonoBehaviour {
    
    static public Unity_input instance;

    public UnityEngine.Input unity;
    
    public Vector2 mouse_world_position { get; private set; }
    
    public Vector2 moving_vector { get; private set; }
    public float scroll_value { get; private set; }
    public int mouse_wheel_steps {
        get {
            return (int) System.Math.Round(scroll_value / 120);
        }
    }
    public bool zoom_held { get; set; }

    public unity.ui.input.mouse.Cursor cursor;

    
    public void Awake() {
        init_singleton();
    }

    private void init_singleton() {
        Contract.Requires(instance == null, "singleton should be initialised once");
        instance = this;
    }
    
    
    public Vector2 read_mouse_world_position()
    {
        return Camera.main.ScreenToWorldPoint(
            UnityEngine.Input.mousePosition
        );

    }

    public bool button_presed(string name) {
        return UnityEngine.Input.GetButtonDown(name);
    }
    
    public bool GetMouseButtonDown(int index) {
        return UnityEngine.Input.GetMouseButtonDown(index);
    }

    private Vector2 read_moving_vector() {
        float horizontal = UnityEngine.Input.GetAxis("Horizontal");
        float vertical = UnityEngine.Input.GetAxis("Vertical");
        
        Vector2 direction_vector = new Vector2(horizontal, vertical);
        
        return direction_vector.normalized;
    }

    private float read_scroll() {
        float wheel_movement = UnityEngine.Input.GetAxis("Mouse ScrollWheel");
        return wheel_movement * 1200;
    }
    

    private void Update() {
        mouse_world_position = read_mouse_world_position();
        moving_vector = read_moving_vector();
        scroll_value = read_scroll();
        zoom_held = true;// UnityEngine.Input.GetButton("Zoom");
    }

    public TComponent get_component_under_mouse<TComponent>() where TComponent: Component {
        Ray ray = new Ray(get_mouse_position_from_top(), Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            if(
                hit.transform.GetComponent<TComponent>() 
                    is TComponent component
            ) { 
                return component;
            }
        }
        return null;
    }
    public Transform get_object_under_mouse() {
        Ray ray = new Ray(get_mouse_position_from_top(), Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            return hit.transform;
        }
        return null;
    }
    private Vector3 get_mouse_position_from_top() {
        return new Vector3(
            mouse_world_position.x,
            mouse_world_position.y,
            -100
        );
    }
    
}

}