using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.general;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Linq;
using rvinowise.ai.unity.visuals;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.ui.input;
using rvinowise.unity.ui.input.mouse;
using TMPro;

namespace rvinowise.ai.unity {

public partial class Figure:
    ISelectable
{

    #region ISelectable

    public void accept_selection(Selector selector) {
        selector.select(this);
    }
    public void accept_deselection(Selector selector) {
        selector.deselect(this);
    }

    [SerializeField] public Renderer selection_sprite_renderer => sprite_renderer;
    [SerializeField] private SpriteRenderer sprite_renderer;
    public new Collider collider{get;private set;}
    #endregion ISelectable

}
}