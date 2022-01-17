using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


    private Figure create_figure_for_sequence_of_subfigures(
        IReadOnlyList<IFigure> subfigures
    ) {
        Figure figure = figure_prefab.get_from_pool<Figure>();
        figure.id = get_id_for(subfigures);

        var representation = figure.create_representation();
        foreach (IFigure child_figure in subfigures) {
            representation.add_subfigure(child_figure);
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

    public IFigure provide_sequence_for_pair(
        IFigure beginning,
        IFigure ending
    ) {
        var subfigures = get_sequence_of_subfigures_from(
            beginning, ending
        );
        return provide_figure_having_sequence(subfigures);
    }

    private IReadOnlyList<IFigure> get_sequence_of_subfigures_from(
        IFigure beginning, IFigure ending
    ) {
        return get_sequence_of_subfigures_from(beginning).Concat(
            get_sequence_of_subfigures_from(ending)
        ).ToList();
    }
    
    private IReadOnlyList<IFigure> get_sequence_of_subfigures_from(
        IFigure figure
    ) {
        if (figure is IPattern pattern) {
            if (pattern.subfigures.Any()) {
                return pattern.subfigures; 
            }
        }
        return new List<IFigure>{figure};
    }
    

}
}