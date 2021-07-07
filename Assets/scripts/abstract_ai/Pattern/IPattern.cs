

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

public class EqualityComparer: IEqualityComparer<IPattern> {

    public bool Equals(IPattern p1, IPattern p2)
    {
        if (
            (p1==null)&&(p2==null)
        ) {
            return true;
        } else if (
            (p1==null)||(p2==null)
        ) {
            return false;
        }
        return p1.id == p2.id;
    }

    public int GetHashCode(IPattern obj)
    {
       return obj.id.GetHashCode();
    }
}
}