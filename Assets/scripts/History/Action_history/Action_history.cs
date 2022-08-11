/* visualises all the actions that were input into the system */
using System.Collections.Generic;
using UnityEngine;
using rvinowise.ai.general;
using System.Linq;
using System.Numerics;
using rvinowise.ai.ui.general;
using rvinowise.unity.ui.input;
using Vector2 = UnityEngine.Vector2;

namespace rvinowise.ai.unity {
public class Action_history:
Visual_input_receiver,
IAction_history
{
 
    public Action_group action_group_prefab;
    public Figure_appearance figure_appearance_prefab;
    public Vector2 action_group_offset = new Vector2(2f,0f);
    public float lines_offset = -4f;
    public Transform caret;
    [SerializeField] private Figure figure_prefab;
    
    private ai.simple.Action_history simple_history;

    #region IAction_history
    public BigInteger last_moment => simple_history.last_moment;

    public IReadOnlyList<IAction_group> get_action_groups(
        BigInteger begin, 
        BigInteger end
    ) => simple_history.get_action_groups(begin, end);

    public IReadOnlyList<IAction_group> get_action_groups() => simple_history.get_action_groups(); 
    
    public IFigure_appearance create_figure_appearance(
        IFigure figure,
        IAction_group start,
        IAction_group end
    ) {
        Figure_appearance appearance = figure_appearance_prefab
            .get_for_figure(figure);
        figure.add_appearance(appearance);
        put_action_into_group(appearance.get_start(), start);
        put_action_into_group(appearance.get_end(), end);
        appearance.create_curved_line();
        return appearance;
    }
    
    public override void input_signals(
        IEnumerable<IFigure> signals, 
        int mood_change =0
    ) => simple_history.input_signals(signals, mood_change);


    public void remove_appearances_of(IFigure figure) => simple_history.remove_appearances_of(figure);

    #endregion IAction_history
    
    void Awake() {
        simple_history = new ai.simple.Action_history {
            create_figure_appearance = create_figure_appearance, 
            create_next_action_group = create_next_action_group
        };
    }
    

    public Action_group create_next_action_group(float in_mood = 0f) {
        Action_group new_group =
            action_group_prefab.get_for_moment(
                last_moment + 1
            );
        new_group.init_mood(in_mood);
        place_new_action_group(new_group);
        simple_history.add_next_action_group(new_group);
        return new_group;
    }


    public IAction_group get_action_group_at_moment(
        BigInteger moment
    ) => simple_history.get_action_group_at_moment(moment);


    public override void start_new_line() {
        caret.position = new Vector2(
            0,
            caret.position.y+lines_offset
        );
    }

    public IReadOnlyList<IAction_group> get_selected_groups() =>
        get_action_groups().Select(
            group => group.    
        )

    private void put_action_into_group(
        IVisual_action action, 
        IAction_group group
    ) {
        group.add_action(action);
        action.action_group = group;
    }


    private void place_new_action_group(Action_group in_group) {
        in_group.transform.parent = transform;
        in_group.transform.position = caret.position;
        caret.Translate(action_group_offset);
    }

}
}