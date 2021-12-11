
using System.Collections.Generic;
using System.Numerics;
using abstract_ai;
using rvinowise.ai.patterns;

namespace rvinowise.ai.patterns {

public interface IAction_group {

    IEnumerator<IAction> GetEnumerator();

    BigInteger moment{get;}
    float mood{get;}
    
    bool has_action<TAction>(IFigure figure) where TAction: IAction;
    void add_action(IAction action);
    void remove_action(IAction action);
}
}