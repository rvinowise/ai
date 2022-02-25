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
    public IFigure figure{get; }
    public BigInteger start_moment { get; set; }
    public BigInteger end_moment { get; set; }

    #endregion IFigure_appearance

    
    public Figure_appearance(IFigure in_figure) {
        this.figure = in_figure;
    }

    

    
}
}