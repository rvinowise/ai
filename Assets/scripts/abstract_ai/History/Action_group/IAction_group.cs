
using System.Collections.Generic;
using System.Numerics;

namespace rvinowise.ai.general {

public interface IAction_group {

    IEnumerator<IAction> GetEnumerator();

    BigInteger moment{get;}
    float mood{get;}
    
    bool has_action<TAction>(IFigure figure) where TAction: IAction;
    void add_action(IAction action);
    void remove_action(IAction action);
}
}