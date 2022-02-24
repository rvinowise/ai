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

namespace rvinowise.ai.simple {
public class Figure_storage: IFigure_storage {
    private readonly List<IFigure> known_figures = new List<IFigure>();
    private Table figure_button_table;


    private Dictionary<string, IFigure> name_to_figure = 
        new Dictionary<string, IFigure>();

    

    public void append_figure(IFigure figure) { 

        known_figures.Add(figure);
        name_to_figure.Add(figure.id, figure);
    }


    public void remove_figure(IFigure figure) {
        known_figures.Remove(figure);
        name_to_figure.Remove(figure.id);
    }

    
    public IFigure find_figure_with_id(string id) {
        IFigure figure;
        name_to_figure.TryGetValue(id, out figure);
        return figure;
    }

}
}