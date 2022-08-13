using System.Collections.Generic;
using System.Numerics;

namespace rvinowise.ai.general {
public interface IHistory_interval {
    BigInteger start_moment{get;}
    BigInteger end_moment{get;} 

    IReadOnlyList<IFigure_appearance> get_figure_appearances(
        IFigure figure
    );

    #region collection interface 
    IEnumerator<IAction_group> GetEnumerator();
    IAction_group this[int i] { get; }
    int Count { get; }
    #endregion
    

}
}