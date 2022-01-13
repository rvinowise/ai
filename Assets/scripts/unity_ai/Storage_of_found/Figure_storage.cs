using System;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.ai.unity;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.table;
using UnityEngine;

namespace rvinowise.ai.unity {
public class Figure_storage: MonoBehaviour {
    public List<IFigure> known_figures = new List<IFigure>();
    public List<IFigure> known_sequential_figures = new List<IFigure>();
    public Table figure_table;
    public IFigure pleasure_signal;
    public IFigure pain_signal;
    
    public Dictionary<string, IFigure> name_to_figure = 
        new Dictionary<string, IFigure>();

    public Figure figure_prefab;
    public int last_id;

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

    public void append_figure(Figure figure) { 
        Figure unity_figure = figure as Figure;

        known_figures.Add(figure);
        name_to_figure.Add(figure.id, figure);
        figure_table.add_item(unity_figure);
    }

    public IFigure add_new_figure() {
        Figure new_figure = figure_prefab.get_from_pool<Figure>();
        new_figure.id = (last_id++).ToString();
        append_figure(new_figure);
        return new_figure;
    }

    public void remove_figure(IFigure figure) {
        known_figures.Remove(figure);
        name_to_figure.Remove(figure.id);
        if (figure is MonoBehaviour unity_figure) {
            figure_table.remove_item(unity_figure);
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
            figure.selected = false;
        }
    }

    public IFigure find_figure_with_id(string id) {
        IFigure figure;
        name_to_figure.TryGetValue(id, out figure);
        return figure;
    }
}
}