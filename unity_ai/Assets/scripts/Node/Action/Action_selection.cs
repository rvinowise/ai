

using System.Linq;
using rvinowise.ai.general;
using rvinowise.ai.ui.general;
using UnityEditor;
using UnityEngine;


namespace rvinowise.ai.unity {

public class Action_selection:
    ai.ui.unity.Selection_of_rendered_object 
{
    private IVisual_action action;

    public Action_selection(SpriteRenderer sprite_renderer) : base(sprite_renderer) { }

}

}