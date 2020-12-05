/* visualises all the actions that were input into the system */
using System.Collections.Generic;
using rvinowise.unity.ai.action;
using rvinowise.ai.action;
using UnityEngine;
using rvinowise.unity.extensions;
using Action = rvinowise.unity.ai.action.Action;
using rvinowise.ai.patterns;

namespace rvinowise.unity.ai.patterns {
public partial class Action_history:
    Input_receiver,
    IHistory_interval
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

    public IHistory_interval get_interval(
        Action_group start, 
        Action_group end
    ) {
        History_interval interval =
            new History_interval(
                this,
                start.moment,
                end.moment
            );
        return interval;
    }
    
    
    public override void input_selected_patterns() {
        Action_group group_start = create_action_group_for_selected_patterns();
        Action_group group_end = create_action_group_for_selected_patterns();

        create_pattern_appearances_fired_in(group);
    }

    private Action_group create_action_group_for_selected_patterns() {
        Action_group new_group =
            action_group_prefab.get_for_selected_patterns_of(pattern_storage);
        place_new_action_group(new_group);
        
        action_groups.Add(new_group);
        return new_group;
    }

    private void create_pattern_appearances_fired_in(
        IAction_group action_group
    ) {
        foreach (var action in action_group) {
            Pattern_appearance appearance =
                pattern_appearance_preafab.get_for_fired_pattern(action);
            
            pattern_appearances.add(
                action.pattern,
                appearance
            );
            
            place_new_pattern_appearance(appearance);
        }
        
    }

    public void add_pattern_appearance(
        IPattern in_pattern,
        IAction start,
        IAction end
    ) {
        pattern_appearances.add(
            in_pattern,
            pattern_appearance_preafab.get_for_interval(
                start,end  
            )
        );
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