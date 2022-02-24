using System;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.ai.unity;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;
using rvinowise.unity.ui.table;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace rvinowise.ai.general {
public interface IFigure_storage {

    public void append_figure(IFigure figure);

    public void remove_figure(IFigure figure);

    public IFigure find_figure_with_id(string id);

    public IReadOnlyList<IFigure> get_known_figures();

}
}