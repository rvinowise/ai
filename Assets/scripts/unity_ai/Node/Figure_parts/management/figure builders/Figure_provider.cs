
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;

namespace rvinowise.ai.unity {

public class Figure_provider:
    IFigure_provider
{

    private Figure figure_prefab;
    private IAction_history action_history;
    private ISequence_builder sequence_builder;
    private Mode_selector mode_selector;

    private ISet<IFigure> known_figures = new HashSet<IFigure>();

    private IReadOnlyList<IFigure> get_known_figures() {
        return known_figures.ToList().AsReadOnly();
    }
    
    public void add_known_figure(IFigure figure) {
        known_figures.Add(figure);
    }
    void Awake() {
    }

    public Figure_provider(
        Figure figure_prefab,
        IAction_history action_history
    ) {
        this.action_history = action_history;
        this.figure_prefab = figure_prefab;
        this.sequence_builder = new Sequence_builder(this);
    }
    
    public Figure_provider(
        Figure figure_prefab,
        IAction_history action_history,
        ISequence_builder sequence_builder,
        Mode_selector mode_selector
    ) {
        this.action_history = action_history;
        this.figure_prefab = figure_prefab;
        this.sequence_builder = sequence_builder;
        this.mode_selector = mode_selector;
    }
    
  
    private Dictionary<string,int> last_ids = new Dictionary<string, int>();


    public IFigure provide_sequence_for_pair(
        IFigure beginning_figure,
        IFigure ending_figure    
    ) {
        var subfigures = sequence_builder.get_sequence_of_subfigures_from(
            beginning_figure, ending_figure
        );
        return provide_figure_having_sequence(subfigures);
    }
    private IFigure provide_figure_having_sequence(
        IReadOnlyList<IFigure> subfigures
    ) {
        if (find_figure_having_sequence(subfigures) is IFigure old_pattern) {
            return old_pattern;
        }
        IFigure new_figure =  sequence_builder.create_figure_for_sequence_of_subfigures(subfigures);
        
        return new_figure;
    }
    public IFigure find_figure_having_sequence(
        IReadOnlyList<IFigure> subfigures
    ) {
        // var action_groups = action_history.get_action_groups(
        //     0,
        //     action_history.last_moment
        // );
        // ISet<IFigure> known_figures = find_familiar_figures(
        //     action_groups
        // );
        
        foreach(var figure in get_known_figures()) {
            if (
                figure.as_lowlevel_sequence().SequenceEqual(subfigures)
            ) {
                return figure;
            }
        }
        return null;
    }
    
    public ISet<IFigure> find_familiar_figures(
        IReadOnlyList<IAction_group> action_groups
    ) {
        ISet<IFigure> result = new HashSet<IFigure>(); 
        foreach (IAction_group group in action_groups) {
            foreach (IAction action in group) {
                result.Add(action.figure);
            }
        }

        return result;
    }
    
    public IFigure create_new_figure(string prefix = "") {
        Figure new_figure = figure_prefab.provide_new<Figure>();
        new_figure.id = get_next_id_for_prefix(prefix);
        new_figure.header.mode_selector = mode_selector;
        return new_figure;
    }

    private string get_next_id_for_prefix(string prefix) {
        int next_id = 0;
        last_ids.TryGetValue(prefix, out next_id);
        last_ids[prefix] = ++next_id;
        return $"{prefix}{next_id}";
    }



  


}
}