
using UnityEngine;

namespace rvinowise.unity.ai {

public class Menu_panel:MonoBehaviour {

    public RectTransform rect;
    void Awake() {
        rect = GetComponent<RectTransform>();
    }

}
}