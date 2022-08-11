using UnityEngine;


namespace rvinowise.ai.unity {

public class Menu_bar : MonoBehaviour {
    private Menu_panel[] panels;
    private Camera camera;
    void Start() {
        panels = gameObject.GetComponentsInChildren<Menu_panel>(true);
        camera = Camera.main;
    }

    void Update() {
        if (UnityEngine.Input.GetMouseButton(0)) {
            foreach (var menu_panel in panels) {
                hide_if_mouse_outside(menu_panel);
            }
        }
    }

    private void hide_if_mouse_outside(Menu_panel panel) {
        if (
            panel.gameObject.activeSelf &&
            !RectTransformUtility.RectangleContainsScreenPoint(
                panel.rect,
                UnityEngine.Input.mousePosition,
                camera
            )
        ) 
        {
            panel.gameObject.SetActive(false);
        }
    }
}

}
