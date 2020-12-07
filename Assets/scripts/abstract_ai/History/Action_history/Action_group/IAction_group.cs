
using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.patterns;

namespace rvinowise.ai.patterns {

public interface IAction_group {

    IEnumerator<IAction> GetEnumerator();

    BigInteger moment{get;}
    
    bool has_action<TAction>(IPattern pattern) where TAction: IAction;
    void add_action(IAction action);
}
}