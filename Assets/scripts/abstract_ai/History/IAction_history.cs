using System.Collections.Generic;
using System.Numerics;
using abstract_ai;

namespace rvinowise.ai.patterns {
public interface IAction_history:
    IFigure,
    IFigure_appearance
{
    
    BigInteger last_moment{get;} 

    IReadOnlyList<IAction_group> get_action_groups(
        BigInteger begin,
        BigInteger end
    );


}
}