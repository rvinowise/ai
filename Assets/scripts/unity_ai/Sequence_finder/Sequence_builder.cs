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

namespace rvinowise.ai.unity {
public class Sequence_builder: 
    ISequence_builder
   //,IFigure_provider
{
    private readonly IFigure_provider figure_provider;

    public Sequence_builder(IFigure_provider figure_provider) {
        this.figure_provider = figure_provider;
    }
    
    
    public IFigure create_sequence_for_pair(
        IFigure beginning,
        IFigure ending
    ) {
        var subfigures = get_sequence_of_subfigures_from(
            beginning, ending
        );
        return create_figure_for_sequence_of_subfigures(subfigures);
    }
    public IFigure create_figure_for_sequence_of_subfigures(
        IReadOnlyList<IFigure> subfigures
    ) {
        IFigure figure = figure_provider.create_new_figure(
            get_id_for(subfigures)
        );
        
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

    public IReadOnlyList<IFigure> get_sequence_of_subfigures_from(
        IFigure beginning, IFigure ending
    ) {
        return beginning.as_lowlevel_sequence().Concat(
            ending.as_lowlevel_sequence()
        ).ToList();
    }
    
    
    // #region IFigure_provider
    //
    // [SerializeField] private Figure figure_prefab;
    // public IFigure create_new_figure(string prefix = "") {
    //     IFigure figure = figure_prefab.provide_new<Figure>();
    //     
    // }
    //
    // #endregion IFigure_provider

}
}