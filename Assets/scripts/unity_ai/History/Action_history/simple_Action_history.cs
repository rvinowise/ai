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

namespace rvinowise.ai.simple {
public partial class Action_history:
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

    public IFigure_appearance create_figure_appearance(
        IFigure figure,
        IAction_group start,
        IAction_group end
    ) {
        Contract.Requires(
            start.moment < end.moment,
            "should have a positive time interval"
        );
        IFigure_appearance appearance = 
            new Figure_appearance(
                figure, start, end);
        figure.add_appearance(appearance);
        put_action_into_group(appearance.get_start(), start);
        put_action_into_group(appearance.get_end(), end);
        return appearance;
    }

    #endregion

    private IList<IAction_group> action_groups = 
        new List<IAction_group>();
    
    private Dictionary<BigInteger, IAction_group> moments_to_action_groups=
        new Dictionary<BigInteger, IAction_group>();

    private Dictionary_of_lists<
        IFigure,
        IFigure_appearance
    > figure_appearances =
        new Dictionary_of_lists<
            IFigure,
            IFigure_appearance
        >();

    private BigInteger current_moment;
    
    
    public IAction_group create_next_action_group(float in_mood = 0f) {
        IAction_group new_group =
            new Action_group(
                current_moment,
                in_mood
            );
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


    private void put_action_into_group(
        IAction action, 
        IAction_group group
    ) {
        Contract.Ensures(
            group != null,
            "first action_group must be created, then actions added to it"
        );
        group.add_action(action);
        
    }

    private IAction_group get_action_group_at_moment(
        BigInteger moment
    ) {
        IAction_group result;
        moments_to_action_groups.TryGetValue(moment,out result);
        return result;
    }




}
}