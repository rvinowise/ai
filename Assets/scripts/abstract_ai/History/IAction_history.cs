using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.general;

namespace rvinowise.ai.general {
public interface IAction_history:
    IInput_receiver
{
    
    BigInteger last_moment{get;} 

    IReadOnlyList<IAction_group> get_action_groups(
        BigInteger begin,
        BigInteger end
    );

    public void input_signals(IEnumerable<IFigure> signals, int mood_change = 0);

    public IFigure_appearance create_figure_appearance(
        IFigure figure,
        IAction_group start,
        IAction_group end
    );

    void remove_appearances_of(IFigure figure);
}
}