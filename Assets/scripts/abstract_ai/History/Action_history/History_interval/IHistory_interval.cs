using System.Collections.Generic;
using System.Numerics;

namespace rvinowise.ai.patterns {
public interface IHistory_interval {
    BigInteger start_moment{get;}
    BigInteger end_moment{get;} 

    IReadOnlyList<IPattern_appearance> get_pattern_appearances(
        IPattern pattern
    );

    #region collection interface 
    IEnumerator<IAction_group> GetEnumerator();
    IAction_group this[int i] { get; }
    int Count { get; }
    #endregion
    

}
}