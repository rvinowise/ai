/* visualises all the actions that were input into the system */
using System.Collections.Generic;
using rvinowise.ai.unity;
using UnityEngine;
using rvinowise.unity.extensions;
using Action = rvinowise.ai.unity.Action;
using rvinowise.ai.general;
using System.Linq;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;

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
        IPattern,
        IPattern_appearance
    > pattern_appearances =
        new Dictionary_of_lists<
            IPattern,
            IPattern_appearance
        >();
    private ulong _count;

    private BigInteger current_moment;
    
    
    public override void input_selected_patterns() {
        var selected_patterns = pattern_storage.get_selected_patterns();
        if (!selected_patterns.Any()) {
            return;
        }
        float new_mood = get_last_mood()+pattern_storage.get_selected_mood();

        Action_group start_group = add_action_group(new_mood);
        Action_group end_group = add_action_group(new_mood);

        create_pattern_appearances(
            selected_patterns,
            start_group.moment,
            end_group.moment
        );
    }

    private Action_group add_action_group(float in_mood = 0f) {
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

    private void create_pattern_appearances(
        IEnumerable<IPattern> patterns,
        BigInteger start,
        BigInteger end
    ) {
        foreach (var pattern in patterns) {
            create_pattern_appearance(pattern, start, end);
        }
        get_action_group_at_moment(start).
            extend_to_accomodate_children();
        get_action_group_at_moment(end).
            extend_to_accomodate_children();
    }

    public void create_pattern_appearance(
        IPattern pattern,
        BigInteger start,
        BigInteger end
    ) {
        IPattern_appearance appearance =
            pattern.create_appearance(start, end);
        
        place_new_pattern_appearance(appearance);
    }

    public Action_group get_action_group_at_moment(
        BigInteger moment
    ) {
        Action_group result;
        moments_to_action_groups.TryGetValue(moment,out result);
        return result;
    }
    
    /* IHistory_interval interface */
    public IReadOnlyList<IPattern_appearance> get_pattern_appearances(
        IPattern pattern    
    ) {
        return pattern_appearances[pattern].AsReadOnly() 
            as IReadOnlyList<IPattern_appearance>;
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