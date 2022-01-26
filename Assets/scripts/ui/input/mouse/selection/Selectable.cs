using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rvinowise;
using rvinowise.rvi.contracts;


namespace rvinowise.unity.ui.input.mouse {

public class Selectable: 
MonoBehaviour,
ISelectable {
    public Component selectable_component;

    void Awake() {
        //selectable_component = GetComponents<ISelectable>().;
    }
    
    #region ISelectable

    public void accept_selection(Selector selector) {
        Selector.select(selectable_component as ISelectable);
    }
    public void accept_deselection(Selector selector) {
        Selector.deselect(selectable_component as ISelectable);
    }

    public Renderer selection_sprite_renderer => sprite_renderer;
    [SerializeField] private SpriteRenderer sprite_renderer;
    public new Collider collider => _collider;
    [SerializeField] private Collider _collider;
    #endregion ISelectable
}
}