using System;
using System.Collections;
using System.Collections.Generic;
using rvinowise.rvi.contracts;
using rvinowise.unity.ui.input;
using UnityEngine;

public class Main_camera : MonoBehaviour {
    
    public float min_zoom = 0.5f;
    public float max_zoom = 100f;

    private float zoom;
    private Camera main_camera;

    
    void Awake() {
        main_camera = GetComponent<Camera>();
        Contract.Requires(main_camera != null, "Main_camera component should be attached only to Cameras");
        Contract.Requires(main_camera.orthographic, "the 2D game should use orthographic cameras only");
        zoom = main_camera.orthographicSize;
    }

    


    void LateUpdate() {
        input_change_zoom();
        
        drag_by_mouse();
    }

    // void Update() {
    //     Zoom();
    // }

    private void input_change_zoom() {
        float wheel_movement = Unity_input.instance.scroll_value;
        
        if (Unity_input.instance.zoom_held 
            &&
            wheel_movement != 0) 
        {
            zoom -= adjust_to_current_zoom(wheel_movement);
            zoom = preserve_possible_zoom(zoom);
            
            Vector3 mouseOnWorld = main_camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            main_camera.orthographicSize = zoom;
            Vector3 mouseOnWorld1 = main_camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

            Vector3 posDiff = mouseOnWorld - mouseOnWorld1;

            Vector3 camPos = main_camera.transform.position;
            Vector3 targetPos = new Vector3(
                camPos.x + posDiff.x,
                camPos.y + posDiff.y,
                camPos.z);

            main_camera.transform.position = targetPos;
        }
    }

    private static float zoom_speed = 0.0016f;
    private void Zoom()
    {
        float mouseScrollWheel = Unity_input.instance.scroll_value;
        
        float newZoomLevel = main_camera.orthographicSize - mouseScrollWheel;

        Vector3 mouseOnWorld = main_camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
        main_camera.orthographicSize = Mathf.Clamp(newZoomLevel, min_zoom, max_zoom);
        Vector3 mouseOnWorld1 = main_camera.ScreenToWorldPoint(UnityEngine.Input.mousePosition);

        Vector3 posDiff = mouseOnWorld - mouseOnWorld1;

        Vector3 camPos = main_camera.transform.position;
        Vector3 targetPos = new Vector3(
            camPos.x + posDiff.x,
            camPos.y + posDiff.y,
            camPos.z);

        main_camera.transform.position = targetPos;
    }
 
    

    
    private float adjust_to_current_zoom(float zoom_delta) {
        //zoom_delta = (zoom / max_zoom) * zoom_delta * zoom_speed;
        zoom_delta = Mathf.Pow(zoom, 0.8f) * zoom_delta * zoom_speed;
        return zoom_delta;
    }

    private float preserve_possible_zoom(float zoom) {
        return Mathf.Clamp(zoom, min_zoom, max_zoom);
    }


    
    private Vector3 click_position;
    private bool is_dragging;
    void drag_by_mouse() {
        bool button_pressed = UnityEngine.Input.GetMouseButton(1);
        if (
            (!is_dragging)&&(button_pressed)
        ) {
            start_dragging();
        }
        if (is_dragging) {
            if (!button_pressed) {
                is_dragging = false;
            } else {
                perform_dragging();
            }
        }

        void start_dragging() {
            click_position = Camera.main.ScreenToWorldPoint (
                UnityEngine.Input.mousePosition
            );
            is_dragging = true;
        }

        void perform_dragging() {
            Vector3 mouse_position = Camera.main.ScreenToWorldPoint (
                UnityEngine.Input.mousePosition
            );
            Vector3 difference = mouse_position -
                Camera.main.transform.position;
            Camera.main.transform.position = 
                click_position-difference;
        }
        
    }

    public float pixels_per_unit() {
        return main_camera.orthographicSize;
    }
}
