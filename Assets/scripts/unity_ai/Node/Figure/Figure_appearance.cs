using System.Numerics;
using rvinowise.ai.general;
using System.Collections.Generic;
using System;
using rvinowise.unity.extensions;


namespace rvinowise.ai.unity {

public class Figure_appearance:
    Repetition_appearance
{

    #region IFigure_apperance
    public string id;
    #endregion IFigure_apperance

    public Figure_appearance get_for_figure(IFigure figure) {
        Figure_appearance appearance = 
            this.get_from_pool<Figure_appearance>();
        
        appearance.figure = figure;
        
        return appearance;
    }

    #region debug
    IList<ISubfigure_appearance> subfigure_appearances 
        = new List<ISubfigure_appearance>();

    #endregion

    
}
}