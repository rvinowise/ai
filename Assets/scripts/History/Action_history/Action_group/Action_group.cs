using System.Collections;
using System.Collections.Generic;
using System.Linq;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.ai.general;
using UnityEngine;
using System.Numerics;
using rvinowise.ai.ui.general;
using TMPro;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;


namespace rvinowise.ai.unity {

public class Action_group:
MonoBehaviour,
IVisual_action_group
{
    private BigInteger _moment; 
    private readonly IList<IAction> actions = new List<IAction>();
  
    #region IAction_group
    
    public IEnumerator<IAction> GetEnumerator() => 
        actions.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    public BigInteger moment{
        get => _moment;
        private set {
            _moment = value;
            moment_label.SetText(value.ToString());
        }
    }

    public float mood {
        get;
        private set;
    }


    public bool has_action(IFigure figure, Action_type type) =>
        actions.Any(action => action.figure == figure && action.type == type);
        
        

    public void add_action(IAction in_action) {
        actions.Add(in_action);
        if (in_action is Action action) {
            place_next_action(action);
        }
        update_shape_accomodating_children();
    }


    public void remove_action(IAction in_action) {
        actions.Remove(in_action);
        update_shape_accomodating_children();
    }

    #endregion IAction_group

    
        
    public void init_mood(float value) {
        mood = value;
        mood_label.set_mood(value);
    }

    [called_by_prefab]
    public Action_group get_for_moment(
    BigInteger moment,
    float mood = 0f
    ) {
        Action_group new_group = this.provide_new<Action_group>();
        new_group.moment = moment;
        new_group.mood = mood;
        return new_group;
    }
    
    #region visualisation
    
    public Vector3 action_offset = new(0,1, 0);
    public Mood_label mood_label;
    public TextMeshPro moment_label;
    [SerializeField]
    SpriteRenderer actions_sprite_renderer;

    [SerializeField]
    Transform actions_attachment;
 
    #region IAccept_selection

    private void Awake() {
        action_group_selection = new Action_group_selection(GetComponent<SpriteRenderer>());
    }
    
    public Selection_of_object selection { get; } = action_group_selection;
    private Action_group_selection action_group_selection;

    #endregion IAccept_selection
    
    private void place_next_action(Action in_action) {
        in_action.transform.parent = this.actions_attachment;
        in_action.transform.localPosition = action_offset * (actions.Count-1);
    }

    private void update_shape_accomodating_children() {
        actions_sprite_renderer.size = 
            new Vector2(
                actions_sprite_renderer.size.x,
                action_offset.y * (actions.Count) + 0.5f
            );

    }

  
    
    #endregion visualisation

}
}