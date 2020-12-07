

using System.Collections.Generic;
using System.Numerics;

namespace rvinowise.ai.patterns {
public interface IPattern {

    string id {
        get;
    }

    IPattern_appearance create_appearance(
        IAction_group start_group,
        IAction_group end_group
    );

    IReadOnlyList<IPattern_appearance> get_appearances_in_interval(
        BigInteger start, BigInteger end
    );

}
}