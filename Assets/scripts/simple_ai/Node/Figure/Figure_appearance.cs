using System.Numerics;
using rvinowise.ai.general;


namespace rvinowise.ai.simple {

public class Figure_appearance:
    IFigure_appearance
{
    
    #region IFigure_appearance
    public IFigure figure{get; }
    public BigInteger start_moment 
        => appearance_start.action_group.moment;
    public BigInteger end_moment 
        => appearance_end.action_group.moment;

    public IAction get_start() => appearance_start;
    public IAction get_end() => appearance_end;

    #endregion IFigure_appearance

    private readonly Action appearance_start;
    private readonly Action appearance_end;

    
    public Figure_appearance(
        IFigure in_figure,
        IAction_group start,
        IAction_group end
    ) {
        figure = in_figure;
        appearance_start = new Action(Action_type.Start,this,start);
        appearance_end = new Action(Action_type.End,this,end);
    }

    

    
}
}