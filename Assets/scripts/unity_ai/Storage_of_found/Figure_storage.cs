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

namespace rvinowise.ai.unity {
public class Figure_storage: MonoBehaviour {
    public List<IFigure> known_figures = new List<IFigure>();
    public List<IFigure> known_sequential_figures = new List<IFigure>();
    public Table figure_table;
    public Figure pleasure_signal;
    public Figure pain_signal;
    
    public Dictionary<string, IFigure> name_to_figure = 
        new Dictionary<string, IFigure>();
    
    

    public Figure figure_prefab;
    

    void Awake() {
        figure_table.init(figure_prefab);
    }

    
    

    public void append_figure(IFigure figure) { 
        Figure unity_figure = figure as Figure;

        known_figures.Add(figure);
        name_to_figure.Add(figure.id, figure);
        figure_table.add_item(unity_figure);
    }

    


    public void remove_figure(IFigure figure) {
        known_figures.Remove(figure);
        name_to_figure.Remove(figure.id);
        if (figure is MonoBehaviour unity_figure) {
            figure_table.remove_item(unity_figure);
        }
    }
    
    
    public IFigure find_figure_with_id(string id) {
        IFigure figure;
        name_to_figure.TryGetValue(id, out figure);
        return figure;
    }
}
}