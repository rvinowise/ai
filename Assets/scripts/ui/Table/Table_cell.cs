using UnityEngine;
using rvinowise.unity.extensions;

namespace rvinowise.unity.ui.table {
public class Table_cell:
MonoBehaviour,
IHave_destructor  {

    public Component item;
    private Canvas canvas;

    void Awake() {
        canvas = GetComponent<Canvas>();
    }

    public void put_item(Component in_item) {
        in_item.transform.SetParent(canvas.transform, false);
        this.item = in_item;
    }
    public void destroy() {
        item.transform.SetParent(null, false);
        this.destroy_object();
    }
}
}