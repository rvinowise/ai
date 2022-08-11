
using System.Collections.Generic;
using System.Numerics;

namespace rvinowise.ai.general {

public interface IAction_group: IEnumerable<IAction> {

    IEnumerator<IAction> GetEnumerator();

    BigInteger moment{get;}
    float mood{get;}
    
    bool has_action(IFigure figure, Action_type type);
    void add_action(IAction action);
    void remove_action(IAction action);
}
}