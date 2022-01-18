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
            start_group.moment,
            end_group.moment
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
        BigInteger start,
        BigInteger end
    ) {
        foreach (var figure in figures) {
            create_figure_appearance(figure, start, end);
        }
        get_action_group_at_moment(start).
            extend_to_accomodate_children();
        get_action_group_at_moment(end).
            extend_to_accomodate_children();
    }

    public IFigure_appearance create_figure_appearance(
        IFigure figure,
        BigInteger start,
        BigInteger end
    ) {
        Contract.Requires(
            start < end,
            "should have a positive time interval"
        );
        Figure_appearance appearance = figure_appearance_prefab
            .get_for_figure(figure);
        figure.add_appearance(appearance);
        put_action_into_moment(appearance.start_appearance, start);
        put_action_into_moment(appearance.end_appearance, end);
        appearance.create_curved_line();
        return appearance;
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
            in_first_half.start_moment,
            in_second_half.end_moment
        );

        return appearance;
    }

    private void put_action_into_moment(
        Action action, 
        BigInteger moment
    ) {
        Action_group group = get_action_group_at_moment(moment);
        Contract.Ensures(
            group != null,
            "first action_group must be creared, then actions added to it"
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
    
    /* IHistory_interval interface */
    public IReadOnlyList<IFigure_appearance> get_figure_appearances(
        IFigure figure    
    ) {
        return figure_appearances[figure].AsReadOnly() 
            as IReadOnlyList<IFigure_appearance>;
    }

    public IEnumerator<Action_group> GetEnumerator() {
        return action_groups.GetEnumerator();
    }

        

    public Action_group this[int i] {
        get { return action_groups[i]; }
    }

    public int Count {
        get => action_groups.Count;
    }
    
    #region IFigure

    public string id { get; }

    public string as_dot_graph() {
        throw new System.NotImplementedException();
    }

    public IReadOnlyList<IFigure_appearance> get_appearances(IFigure in_where) {
        Contract.Assert(false, "Action history is retrieved via the 'instance' field");
        return null;
    }

    public IReadOnlyList<IFigure_appearance> get_appearances_in_interval(BigInteger start, BigInteger end) {
        throw new System.NotImplementedException();
    }

    #endregion

    #region IFigure_appearance

    public IFigure figure { get; }
    public IFigure place { get; } = null;
    public BigInteger start_moment { get; }
    public BigInteger end_moment { get; }

    #endregion
}
}