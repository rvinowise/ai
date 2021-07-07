

using rvinowise.ai.patterns;
using rvinowise.unity.ai;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using UnityEngine;

namespace rvinowise.unity.ai.action {


public partial class Action: 
IAction
{

    public IPattern pattern{get;private set;}
    public IAction_group action_group{get;private set;}
    public Pattern_appearance pattern_appearance;

    void Start() {
        pattern = pattern_appearance.pattern;
        set_label(pattern.id);
    }

    void OnMouseDown() {
        this.pattern_appearance.selected = !this.pattern_appearance.selected;
        
    }

    public virtual Action put_into_moment(
        IAction_group in_action_group
    ) {
        in_action_group.add_action(this);
        action_group = in_action_group;
        return this;
    }

    public void destroy()
    {
        action_group.remove_action(this);
        ((MonoBehaviour)this).destroy();
    }
   
}
}