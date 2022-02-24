using System.Numerics;
using rvinowise.ai.general;
using System.Collections.Generic;
using System;
using rvinowise.unity;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.extensions.pooling;
using rvinowise.unity.ui.input;
using rvinowise.unity.ui.input.mouse;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


namespace rvinowise.ai.simple {

public class Figure_appearance:
    IFigure_appearance
{
    
    #region IFigure_appearance
    public IFigure figure{get; protected set;}
    public BigInteger start_moment 
        => appearance_start.action_group.moment;
    public BigInteger end_moment 
        => appearance_end.action_group.moment;

    #endregion IFigure_appearance

    
    public Figure_appearance(IFigure in_figure) {
        this.figure = in_figure;
    }
    
    public static Figure_appearance get_for_figure(IFigure figure) {
        Figure_appearance appearance = 
            new Figure_appearance(figure);

        return appearance;
    }
    

    #region debug
    IList<ISubfigure_appearance> subfigure_appearances 
        = new List<ISubfigure_appearance>();

    #endregion

    
}
}