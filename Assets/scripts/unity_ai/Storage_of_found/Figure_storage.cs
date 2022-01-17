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
    

    
    public IEnumerable<Figure> get_selected_figures() {
        IList<Figure> result = new List<Figure>();
        foreach(Figure figure in known_figures) {
            if (figure.selected) {
                result.Add(figure);
            }
        }
        return result;
    }
    
    public float get_selected_mood() {
        Contract.Requires(
            !pleasure_signal.selected ||
            pleasure_signal.selected != pain_signal.selected,
            "either pain or pleasure at the same time"
        );
        if (pleasure_signal.selected) {
            return 1f;
        } else if (pain_signal.selected) {
            return -1f;
        }
        return 0f;
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

    #region selecting figures
    public void select_figures_from_string(string in_string) {
        deselect_all_figures();
        string[] names = in_string.Split(' ')
            .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        foreach (string name in names) {
            IFigure figure;
            name_to_figure.TryGetValue(name, out figure);
            if (figure != null) {
                select_figure((Figure)figure);
            } else {
                Debug.Log($"trying to select non-existing figure \"{name}\"");
            }
        }
    }

    public void select_figure(Figure figure) {
        figure.selected = true;
    }

    public void toggle_figure_selection(Figure figure) {
        figure.selected = !figure.selected;
    }

    public void deselect_all_figures() {
        foreach (var figure in get_selected_figures()) {
            //figure.selected = false;
            Selector.deselect(figure);
        }
    }

    #endregion selecting figures
    
    public IFigure find_figure_with_id(string id) {
        IFigure figure;
        name_to_figure.TryGetValue(id, out figure);
        return figure;
    }
}
}