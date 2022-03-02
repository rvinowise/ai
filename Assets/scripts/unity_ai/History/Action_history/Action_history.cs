/* visualises all the actions that were input into the system */
using System.Collections.Generic;
using rvinowise.ai.unity;
using UnityEngine;
using rvinowise.unity.extensions;
using Action = rvinowise.ai.unity.Action;
using rvinowise.ai.general;
using System.Linq;
using System.Numerics;
using rvinowise.rvi.contracts;
using rvinowise.unity.ui.input;

namespace rvinowise.ai.unity {
public partial class Action_history:
Visual_input_receiver,
IAction_history
{
    ai.simple.Action_history simple_history;

    #region IAction_history
    public BigInteger last_moment => simple_history.last_moment;

    public IReadOnlyList<IAction_group> get_action_groups(
        BigInteger begin, 
        BigInteger end
    ) => simple_history.get_action_groups(begin, end);

    public IFigure_appearance create_figure_appearance(
        IFigure figure,
        IAction_group start,
        IAction_group end
    ) {
        Figure_appearance appearance = figure_appearance_prefab
            .get_for_figure(figure);
        figure.add_appearance(appearance);
        put_action_into_group(appearance.appearance_start, start);
        put_action_into_group(appearance.appearance_end, end);
        appearance.create_curved_line();
        return appearance;
    }
    
    public void input_signals(
        IEnumerable<IFigure> signals, 
        int mood_change =0
    ) => simple_history.input_signals(signals, mood_change);

    #endregion IAction_history
    
    void Awake() {
        simple_history = new ai.simple.Action_history();
        simple_history.create_figure_appearance = create_figure_appearance;
        simple_history.create_next_action_group = create_next_action_group;
    }
    
    public override void input_selected_signals() {
        var selected_figures = Selector.instance.figures;
        if (!selected_figures.Any()) {
            return;
        }
        input_signals(selected_figures);
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



    
    private void put_action_into_group(
        Action action, 
        IAction_group group
    ) {
        group.add_action(action);
        action.action_group = group;
    }

    public IAction_group get_action_group_at_moment(
        BigInteger moment
    ) => simple_history.get_action_group_at_moment(moment);




}
}