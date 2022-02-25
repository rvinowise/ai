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

    #region IAction_history
    public BigInteger last_moment => current_moment - 1;

    public IReadOnlyList<IAction_group> get_action_groups(
        BigInteger begin, 
        BigInteger end
    ) {
        List<IAction_group> result = action_groups.Where(
            action_group => 
            (action_group.moment >= begin) &&
            (action_group.moment <= end)
        ).ToList<IAction_group>();

        return result.AsReadOnly();
    }

    public IFigure_appearance create_figure_appearance(
        IFigure figure,
        IFigure_appearance in_first_half,
        IFigure_appearance in_second_half
    ) {
        Contract.Assert(
            figure.id.Length > 1, 
            "Pattern consisting of subfigures should have a longer name"
        );
        
        IFigure_appearance appearance = create_figure_appearance(
            figure,
            in_first_half.get_start().action_group,
            in_second_half.get_end().action_group
        );

        return appearance;
    }

    // public IFigure_appearance create_figure_appearance(
    //     IFigure figure,
    //     BigInteger start,
    //     BigInteger end
    // ) {
    //     Contract.Requires(
    //         start < end,
    //         "should have a positive time interval"
    //     );
    //     Figure_appearance appearance = figure_appearance_prefab
    //         .get_for_figure(figure);
    //     figure.add_appearance(appearance);
    //     put_action_into_moment(appearance.appearance_start, start);
    //     put_action_into_moment(appearance.appearance_end, end);
    //     appearance.create_curved_line();
    //     return appearance;
    // }

    public IFigure_appearance create_figure_appearance(
        IFigure figure,
        IAction_group start,
        IAction_group end
    ) {
        Contract.Requires(
            start.moment < end.moment,
            "should have a positive time interval"
        );
        Figure_appearance appearance = figure_appearance_prefab
            .get_for_figure(figure);
        figure.add_appearance(appearance);
        put_action_into_group(appearance.appearance_start, start);
        put_action_into_group(appearance.appearance_end, end);
        appearance.create_curved_line();
        return appearance;
    }

    #endregion

    public static Action_history instance {get;private set;}

    private IList<Action_group> action_groups = 
        new List<Action_group>();
    
    private Dictionary<BigInteger, Action_group> moments_to_action_groups=
        new Dictionary<BigInteger, Action_group>();

    private Dictionary_of_lists<
        IFigure,
        IFigure_appearance
    > figure_appearances =
        new Dictionary_of_lists<
            IFigure,
            IFigure_appearance
        >();

    private BigInteger current_moment;
    
    
    public override void input_selected_figures() {
        var selected_figures = Selector.instance.figures;
        if (!selected_figures.Any()) {
            return;
        }
        float new_mood = get_last_mood()+Selector.instance.get_selected_mood();

        Action_group start_group = create_next_action_group(new_mood);
        Action_group end_group = create_next_action_group(new_mood);

        create_figure_appearances(
            selected_figures,
            start_group,
            end_group
        );
    }

    public Action_group create_next_action_group(float in_mood = 0f) {
        Action_group new_group =
            action_group_prefab.get_for_moment(
                current_moment
            );
        new_group.init_mood(in_mood);
        place_new_action_group(new_group);
        action_groups.Add(new_group);
        moments_to_action_groups.Add(current_moment, new_group);
        current_moment++;
        return new_group;
    }

    private float get_last_mood() {
        if (action_groups.Any()) {
            return action_groups.Last().mood;
        }
        return 0f;
    }

    private void create_figure_appearances(
        IEnumerable<IFigure> figures,
        IAction_group start,
        IAction_group end
    ) {
        foreach (var figure in figures) {
            create_figure_appearance(figure, start, end);
        }

    }


    private void put_action_into_moment(
        Action action, 
        BigInteger moment
    ) {
        Action_group group = get_action_group_at_moment(moment);
        put_action_into_group(action, group);
    }
    private void put_action_into_group(
        Action action, 
        IAction_group group
    ) {
        Contract.Ensures(
            group != null,
            "first action_group must be created, then actions added to it"
        );
        group.add_action(action);
        action.action_group = group;
    }

    public Action_group get_action_group_at_moment(
        BigInteger moment
    ) {
        Action_group result;
        moments_to_action_groups.TryGetValue(moment,out result);
        return result;
    }




}
}