using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using rvinowise.ai.general;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.unity.extensions;
using rvinowise.ai.unity.persistence;
using rvinowise.unity.ui.table;
using UnityEngine;
using UnityEngine.UI;

namespace rvinowise.ai.unity {
public class Sequence_builder: MonoBehaviour {
    public Figure figure_prefab;
    public Figure_storage figure_storage;

    private IReadOnlyList<IFigure> known_figures => figure_storage.known_figures;


    private Figure create_signal_of_base_input(
        string id
    ) {
        Figure figure = figure_prefab.get_from_pool<Figure>();
        figure.id = id;
        figure.name = $"pattern {figure.id}";
        return figure;
    }

    public IFigure provide_signal_of_base_input(
        string id
    ) {
        if (find_signal_of_base_input(id) is IFigure old_figure) {
            return old_figure;
        }
        Figure new_signal = create_signal_of_base_input(id);
        figure_storage.append_figure(new_signal);
        return new_signal;
    } 

    public IFigure find_signal_of_base_input(
        string id
    ) {
        foreach(var pattern in known_figures) {
            if (
                pattern.id == id
            ) {
                return pattern;
            }
        }
        return null;
    }

    private Figure create_figure_for_sequence_of_subfigures(
        IReadOnlyList<IFigure> subfigures
    ) {
        Figure figure = figure_prefab.get_from_pool<Figure>();
        figure.id = get_id_for(subfigures);

        foreach (IFigure child_figure in subfigures) {
            figure.add_subfigure(child_figure);
        }
        //pattern.subfigures = subfigures;

        return figure;
    }
    public static string get_id_for(IReadOnlyList<IFigure> subfigures) {
        StringBuilder res = new StringBuilder();
        foreach (var subfigure in subfigures) {
            res.Append(subfigure.id);
        }

        return res.ToString();
    }

    public IFigure provide_figure_having_sequence(
        IReadOnlyList<IFigure> subfigures
    ) {
        if (find_figure_having_sequence(subfigures) is IFigure old_pattern) {
            return old_pattern;
        }
        Figure new_figure = create_figure_for_sequence_of_subfigures(subfigures);
        figure_storage.append_figure(new_figure);
        return new_figure;
    }

    public IFigure find_figure_having_sequence(
        IReadOnlyList<IFigure> subfigures
    ) {
        foreach(var figure in known_figures) {
            if (
                figure.as_lowlevel_sequence().SequenceEqual(subfigures)
            ) {
                return figure;
            }
        }
        return null;
    }

    public IFigure provide_pattern_for_pair(
        IFigure beginning,
        IFigure ending
    ) {
        var subfigures = Pattern.get_sequence_of_subfigures_from(
            beginning, ending
        );
        return provide_figure_having_sequence(subfigures);
    }

    

}
}