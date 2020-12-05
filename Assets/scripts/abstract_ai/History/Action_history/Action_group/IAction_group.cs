
using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.patterns;

namespace rvinowise.ai.action {

public interface IAction_group {

    IEnumerator<IAction> GetEnumerator();

    BigInteger moment{get;}
    
    bool has_action(IPattern pattern, action.type_t type);
    
}
}