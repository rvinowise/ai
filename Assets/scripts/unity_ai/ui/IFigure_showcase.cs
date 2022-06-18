using System;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.ai.simple;
using rvinowise.ai.ui.general;
using rvinowise.ai.ui.unity;
using rvinowise.rvi.contracts;
using rvinowise.ai.unity;
using rvinowise.ai.unity.simple;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;
using rvinowise.unity.ui.table;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace rvinowise.ai.ui.unity {
public interface IFigure_showcase
{

    
    public IVisual_figure shown_figure { get; }

    public void show_insides_of_one_figure(IVisual_figure new_shown_figure);

    #region testing

    IFigure_button get_button_for_figure(IVisual_figure figure);

    #endregion testing
    
}
}