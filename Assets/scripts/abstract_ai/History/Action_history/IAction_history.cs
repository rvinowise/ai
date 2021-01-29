using System.Collections.Generic;
using System.Numerics;

namespace rvinowise.ai.patterns {
public interface IAction_history {
    
    BigInteger last_moment{get;} 

    IReadOnlyList<IAction_group> get_action_groups(
        BigInteger begin,
        BigInteger end
    );


}
}