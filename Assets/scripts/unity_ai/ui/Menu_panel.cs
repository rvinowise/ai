
using UnityEngine;

namespace rvinowise.ai.unity {

public class Menu_panel:MonoBehaviour {

    public RectTransform rect;
    void Awake() {
        rect = GetComponent<RectTransform>();
    }

}
}