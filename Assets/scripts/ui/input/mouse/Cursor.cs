using UnityEngine;


namespace rvinowise.unity.ui.input.mouse {

public class Cursor: MonoBehaviour {



    private SpriteRenderer sprite_renderer;
    

    void Update() {
        transform.position = Unity_input.instance.mouse_world_position;
    }

}
}