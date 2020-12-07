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

    public BigInteger start_moment{get; protected set;}
    public BigInteger end_moment{get; protected set;}

    private IList<Action_group> groups = new List<Action_group>();
    private readonly BigInteger max_interval = int.MaxValue;
    public History_interval(
        BigInteger start_moment,
        BigInteger end_moment
    ) {
        Contract.Requires(
            end_moment - start_moment <= max_interval,
            "a history interval should be small enough to iterate through"
        );
        Contract.Requires(start_moment < end_moment);
        this.start_moment = start_moment;
        this.end_moment = end_moment;
    }
    
    /* IHistory_interval interface */
    
    public IReadOnlyList<IPattern_appearance> get_pattern_appearances(
        IPattern pattern
    ) {
        return pattern_appearances[pattern].AsReadOnly();
    }

    public IEnumerator<IAction_group> GetEnumerator() {
        for (int i=0;i<groups.Count;i++) {
            yield return groups[i];
        }
    }

    public IAction_group this[int i] {
        get { return groups[i]; }
    }

    public int Count {
        get { return groups.Count; }
    }
}
}