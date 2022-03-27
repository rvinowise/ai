
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;

namespace rvinowise.ai.simple {

public class Figure_provider:
    IFigure_provider
{

    private readonly ISequence_builder sequence_builder;

    private readonly ISet<IFigure> known_figures = new HashSet<IFigure>();
    private Dictionary<string, IFigure> name_to_figure = 
        new Dictionary<string, IFigure>();
    
    
    
    public Figure_provider() {
        this.sequence_builder = new Sequence_builder();
    }
    public Figure_provider(
        ISequence_builder sequence_builder
    ) {
        this.sequence_builder = sequence_builder;
    }
    
  
    private Dictionary<string,int> last_ids = new Dictionary<string, int>();

    #region IFigure_provider
    
    public IReadOnlyList<IFigure> get_known_figures() {
        return known_figures.ToList().AsReadOnly();
    }

    public IFigure provide_sequence_for_pair(
        IFigure beginning_figure,
        IFigure ending_figure    
    ) {
        var subfigures = sequence_builder.get_sequence_of_subfigures_from(
            beginning_figure, ending_figure
        );
        return provide_figure_having_sequence(subfigures);
    }

    public IFigure create_figure(string prefix = "") {
        Figure new_figure = new simple.Figure(
            get_next_id_for_prefix(prefix)
        );
        return new_figure;
    }

    public IFigure create_base_signal(string id = "") {
        IFigure signal = create_figure("signal");

        return signal;
    }

    public IFigure find_figure_with_id(string id) {
        name_to_figure.TryGetValue(id, out IFigure figure);
        return figure;
    }

    public void remove_figure(IFigure figure) {
        known_figures.Remove(figure);
        name_to_figure.Remove(figure.id);
    }

    #endregion IFigure_provider
    
    #region used by derived
    public string get_next_id_for_prefix(string prefix) {
        last_ids.TryGetValue(prefix, out var next_id);
        last_ids[prefix] = ++next_id;
        return $"{prefix}{next_id}";
    }
    #endregion used by derived
    
    private IFigure provide_figure_having_sequence(
        IReadOnlyList<IFigure> subfigures
    ) {
        if (find_figure_having_sequence(subfigures) is IFigure old_pattern) {
            return old_pattern;
        }
        IFigure new_figure =  sequence_builder.create_figure_for_sequence_of_subfigures(subfigures);
        add_known_figure(new_figure);
        return new_figure;
    }
    
    private void add_known_figure(IFigure figure) {
        known_figures.Add(figure);
        name_to_figure[figure.id] = figure;
    }
    private IFigure find_figure_having_sequence(
        IReadOnlyList<IFigure> subfigures
    ) {

        foreach(var figure in get_known_figures()) {
            if (
                figure.as_lowlevel_sequence().SequenceEqual(subfigures)
            ) {
                return figure;
            }
        }
        return null;
    }
    

    
    

    



  


}
}