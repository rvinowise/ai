using System;
using UnityEngine;
using rvinowise.unity;
using rvinowise.unity.extensions;

namespace rvinowise.unity.ui.table {
public class Table_cell:
MonoBehaviour,
IHave_destructor  {

    public MonoBehaviour item;
    private Canvas canvas;

    void Awake() {
        canvas = GetComponent<Canvas>();
    }

    public void put_item(MonoBehaviour in_item) {
        in_item.transform.SetParent(canvas.transform, false);
        this.item = in_item;
    }
    public void destroy() {
        this.item.transform.SetParent(null, false);
        ((MonoBehaviour)this).destroy();
    }
}
}