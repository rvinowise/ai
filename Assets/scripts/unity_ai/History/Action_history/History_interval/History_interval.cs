using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.patterns;
using rvinowise.rvi.contracts;
using rvinowise.unity.ai.action;
using rvinowise.unity.extensions;
using UnityEngine.Assertions;

namespace rvinowise.unity.ai.patterns {
public class History_interval: 
    IHistory_interval 
{
    
    private Dictionary_of_lists<
        IPattern,
        IPattern_appearance
    > pattern_appearances =
        new Dictionary_of_lists<
            IPattern,
            IPattern_appearance
        >();

    public IHistory_interval parent_interval;
    public BigInteger start_moment{get; protected set;}
    public BigInteger end_moment{get; protected set;}

    readonly BigInteger max_interval = int.MaxValue;
    public History_interval(
        IHistory_interval parent,
        BigInteger start_moment,
        BigInteger end_moment
    ) {
        Contract.Requires(
            end_moment - start_moment <= max_interval,
            "a history interval should be small enough to iterate through"
        );
        Contract.Requires(start_moment < end_moment);
        Contract.Requires(parent.Count < end_moment);
        this.parent_interval = parent;
        this.start_moment = start_moment;
        this.end_moment = end_moment;
    }
    
    /* IHistory_interval interface */
    
    public IReadOnlyList<IPattern_appearance> get_pattern_appearances(
        IPattern pattern
    ) {
        return pattern_appearances[pattern].AsReadOnly();
    }

    public IEnumerator<Action_group> GetEnumerator() {
        for (int i=start_moment;i<end_moment;i++) {
            yield return parent_interval[i];
        }
    }

    public Action_group this[int i] {
        get { return parent_interval[start_moment + i]; }
    }

    public int Count {
        get { return end_moment - start_moment; }
    }
}
}