/* visualises all the actions that were input into the system */
using System.Collections.Generic;
using rvinowise.unity.ai.action;
using UnityEngine;
using rvinowise.unity.extensions;
using Action = rvinowise.unity.ai.action.Action;
using rvinowise.ai.patterns;
using System.Linq;
using System.Numerics;

namespace rvinowise.unity.ai.patterns {
public partial class Action_history:
    Input_receiver
{

    [HideInInspector]
    public IList<Action_group> action_groups = 
        new List<Action_group>();

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

    public IHistory_interval get_interval(
        Action_group start, 
        Action_group end
    ) {
        History_interval interval =
            new History_interval(
                start.moment,
                end.moment
            );
        return interval;
    }
    
    
    public override void input_selected_patterns() {
        var selected_patterns = pattern_storage.get_selected_patterns();
        if (!selected_patterns.Any()) {
            return;
        }
        Action_group group_start = create_action_group_for_moment(
            current_moment++
        );
        Action_group group_end = create_action_group_for_moment(
            current_moment++
        );

        create_pattern_appearances(
            selected_patterns,
            group_start,
            group_end
        );
    }

    private Action_group create_action_group_for_moment(
        BigInteger moment
    ) {
        Action_group new_group =
            action_group_prefab.get_for_moment(
                moment
            );
        place_new_action_group(new_group);
        
        action_groups.Add(new_group);
        return new_group;
    }

    private void create_pattern_appearances(
        IEnumerable<Pattern> patterns,
        Action_group group_start,
        Action_group group_end
    ) {
        foreach (var pattern in patterns) {
            Pattern_appearance appearance =
                pattern.create_appearance(group_start, group_end);
            
            place_new_pattern_appearance(appearance);
        }
        group_start.extend_to_accomodate_children();
        group_end.extend_to_accomodate_children();
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
}
}