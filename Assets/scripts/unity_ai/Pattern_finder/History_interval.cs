using System.Collections.Generic;
using System.Linq;
using rvinowise.unity.ai.action;

namespace rvinowise.unity.ai.patterns {
class History_interval {
    public Action_history full_history;
    public int begin;
    public int end;
    public IList<Action_group> action_groups;

    public History_interval(
        Action_history in_history,
        int in_begin,
        int in_end    
    ) {
        full_history = in_history;
        begin = in_begin;
        end = in_end;

        action_groups = in_history.action_groups.
            Skip(in_begin).
            Take(in_end-in_begin).ToList();
    }

    public IEnumerator<Action_group> GetEnumerator() {
        return action_groups.GetEnumerator();
    }
}
}