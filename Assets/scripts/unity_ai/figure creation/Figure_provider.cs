
using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;

namespace rvinowise.ai.simple {

public class Figure_provider<TFigure>:
    IFigure_provider<TFigure> where TFigure: class?, IFigure 
{

    private readonly ISequence_builder sequence_builder;

    private readonly ISet<TFigure> known_figures = new HashSet<TFigure>();
    private readonly Dictionary<string, TFigure> name_to_figure = 
        new Dictionary<string, TFigure>();
    
    public delegate TFigure Create_figure(string id);

    private readonly Create_figure create_figure_delegate;
    
    public Figure_provider(
        Create_figure create_figure
    ) {
        sequence_builder = new Sequence_builder();
        create_figure_delegate = create_figure;
    }
    public Figure_provider(
        Create_figure create_figure,
        ISequence_builder sequence_builder
    ) {
        this.sequence_builder = sequence_builder;
        create_figure_delegate = create_figure;
    }
    
  
    private Dictionary<string,int> last_ids = new Dictionary<string, int>();

    #region IFigure_provider

    public TFigure provide_figure(string id) {

        TFigure figure = find_figure_with_id(id) ?? create_new_figure(id);

        return figure;
    }

    public TFigure provide_sequence_for_pair(
        IFigure beginning_figure,
        IFigure ending_figure    
    ) {
        var subfigures = sequence_builder.get_sequence_of_subfigures_from(
            beginning_figure, ending_figure
        );
        return provide_figure_having_sequence(subfigures);
    }

    public IReadOnlyList<TFigure> get_known_figures() {
        return known_figures.ToList().AsReadOnly();
    }


    public TFigure find_figure_with_id(string id) {
        name_to_figure.TryGetValue(id, out TFigure figure);
        return figure;
    }

    public void remove_figure(IFigure in_figure) {
        rvi.contracts.Contract.Requires(in_figure is TFigure);
        if (in_figure is TFigure figure) {
            known_figures.Remove(figure);
            name_to_figure.Remove(figure.id);
        }
    }

    #endregion IFigure_provider
    
    

    private TFigure create_new_figure(string id) {
        TFigure figure = create_figure_delegate(id);
        remember_figure(figure);
        return figure;
    }
    
    public string get_next_id_for_prefix(string prefix) {
        last_ids.TryGetValue(prefix, out var next_id);
        last_ids[prefix] = ++next_id;
        return $"{prefix}{next_id}";
    }


    private TFigure provide_figure_having_sequence(
        IReadOnlyList<IFigure> subfigures
    ) {
        if (find_figure_having_sequence(subfigures) is TFigure old_pattern) {
            return old_pattern;
        }
        TFigure new_figure = provide_figure(sequence_builder.get_id_for(subfigures));
        sequence_builder.add_sequential_representation(new_figure, subfigures);
        remember_figure(new_figure);
        return new_figure;
    }

    private void remember_figure(TFigure figure) {
        known_figures.Add(figure);
        name_to_figure[figure.id] = figure;
    }
    public TFigure find_figure_having_sequence(
        IReadOnlyList<IFigure> subfigures
    ) {
        foreach(var figure in get_known_figures()) {
            var sequence = figure.as_lowlevel_sequence();
            if (
                sequence.SequenceEqual(subfigures)
            ) {
                return figure;
            }
        }
        return null;
    }

}
}