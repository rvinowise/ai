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
using rvinowise.unity.ui.input;

namespace rvinowise.ai.simple {
public class Sequence_builder: 
    ISequence_builder
{
    private IFigure_storage figure_storage;

    private IReadOnlyList<IFigure> known_figures => figure_storage.get_known_figures();

    public Sequence_builder(IFigure_storage figure_storage) {
        this.figure_storage = figure_storage;
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
    
    private IFigure create_figure_for_sequence_of_subfigures(
        IReadOnlyList<IFigure> subfigures
    ) {
        IFigure figure = new Figure( get_id_for(subfigures));

        var representation = figure.create_representation();
        ISubfigure previous = null;
        foreach (IFigure child_figure in subfigures) {
            ISubfigure next = representation.create_subfigure(child_figure);
            previous?.connext_to_next(next);
            previous = next;
        }
        figure.set_lowlevel_sequence(subfigures.ToList());

        return figure;
    }

    private static string get_id_for(IReadOnlyList<IFigure> subfigures) {
        StringBuilder res = new StringBuilder();
        foreach (var subfigure in subfigures) {
            res.Append(subfigure.id);
        }

        return res.ToString();
    }

    private IFigure provide_figure_having_sequence(
        IReadOnlyList<IFigure> subfigures
    ) {
        if (find_figure_having_sequence(subfigures) is IFigure old_pattern) {
            return old_pattern;
        }
        IFigure new_figure = create_figure_for_sequence_of_subfigures(subfigures);
        
        return new_figure;
    }

    private IFigure find_figure_having_sequence(
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

    

    private IReadOnlyList<IFigure> get_sequence_of_subfigures_from(
        IFigure beginning, IFigure ending
    ) {
        return beginning.as_lowlevel_sequence().Concat(
            ending.as_lowlevel_sequence()
        ).ToList();
    }
    

}
}