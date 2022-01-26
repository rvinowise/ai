using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rvinowise;
using rvinowise.rvi.contracts;


namespace rvinowise.unity.ui.input.mouse {

public class Cursor: MonoBehaviour {



    private SpriteRenderer sprite_renderer;
    

    void Update() {
        transform.position = Unity_input.instance.mouse_world_position;
    }

}
}