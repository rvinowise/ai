

using System.Collections.Generic;

namespace rvinowise.ai.patterns {
public interface IPattern_finder {


    void find_new_patterns(IHistory_interval interval);

    IReadOnlyList<IPattern_appearance> find_repeated_pairs(
        IReadOnlyList<IPattern_appearance> beginnings,
        IReadOnlyList<IPattern_appearance> endings
    );


    

}
}