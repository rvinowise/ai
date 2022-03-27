
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.ui.input;

namespace rvinowise.ai.unity {

public class Figure_builder_from_signals
{

    private readonly IFigure_provider figure_provider;
    
    private IFigure figure; //which is being built by this builder
    private IFigure_representation representation; //which is being built by this builder
    private readonly List<ISubfigure> all_subfigures = new List<ISubfigure>();
    private readonly List<ISubfigure> ended_subfigures = new List<ISubfigure>();
    
    private readonly Dictionary<IFigure_appearance, ISubfigure> appearance_to_subfigure 
    = new Dictionary<IFigure_appearance, ISubfigure>();

    private int last_subfigure_id;

    public Figure_builder_from_signals(
        IFigure_provider figure_provider
    ) {
        this.figure_provider = figure_provider;
    }
    
    

    public IFigure create_figure_from_action_history(
        IReadOnlyList<IAction_group> action_groups
    ) {
        clear();
        figure = figure_provider.create_figure("f") as Figure;
        representation = figure.create_representation();
        
        foreach(IAction_group group in action_groups) {
            parce_actions_of(group);
        }
        return figure;
    }
    

    private void clear() {
        appearance_to_subfigure.Clear();
        all_subfigures.Clear();
        ended_subfigures.Clear();
        figure = null;
        representation = null;
        last_subfigure_id = 0;
    }
    private void parce_actions_of(IAction_group group) {
        foreach(IAction action in group) {
            if (action is IAppearance_start) {
                add_next_subfigure(action.figure_appearance);
            } else if (action is IAppearance_end) {
                remember_finished_subfigure(action.figure_appearance);    
            } 
        }
    }

    private void add_next_subfigure(
        IFigure_appearance appended_figure
    ) {
        ISubfigure new_subfigure = representation.create_subfigure(appended_figure.figure);
        new_subfigure.id = (last_subfigure_id++).ToString();
        appearance_to_subfigure.Add(appended_figure, new_subfigure);
        if (ended_subfigures.Any()) {
            foreach (ISubfigure ended_subfigure in ended_subfigures) {
                ended_subfigure.connext_to_next(new_subfigure);
            }
        }
        else {
            representation.add_first_subfigures(new_subfigure);
        }
    }

    private void remember_finished_subfigure(
        IFigure_appearance finished_appearance
    ) {
        appearance_to_subfigure.TryGetValue(
            finished_appearance, out var started_subfigure
        );
        if (started_subfigure != null) {
            ended_subfigures.Add(started_subfigure);
        }
    }


}
}