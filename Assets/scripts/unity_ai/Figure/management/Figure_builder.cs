
using UnityEngine;
using System.Collections.Generic;
using System.Numerics;
using abstract_ai;
using rvinowise.ai.patterns;
using rvinowise.rvi.contracts;

namespace rvinowise.unity.ai.figure {

public class Figure_builder: MonoBehaviour {

    public Action_history action_history;
    public Figure_storage figure_storage; 

    private Figure figure;
    private List<ISubfigure> all_subfigures = new List<ISubfigure>();
    private List<ISubfigure> finished_subfigures = new List<ISubfigure>();
    
    private Dictionary<IFigure_appearance, ISubfigure> 
    appearance_to_subfigure 
    = new Dictionary<IFigure_appearance, ISubfigure>();


    public void on_create_figure_from_actions() {
        //create_figure_from_action_history();
    }

    public IFigure create_figure_from_action_history(
        IReadOnlyList<IAction_group> action_groups
    ) {
        figure = new Figure();
        
        foreach(IAction_group group in action_groups) {
            parce_actions_of(group);
        }
        return figure;
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
        ISubfigure new_subfigure = new Subfigure(appended_figure.figure);
        foreach(ISubfigure ended_subfigure in finished_subfigures) {
            ended_subfigure.connext_to_next(new_subfigure);
        }
    }

    private void remember_finished_subfigure(
        IFigure_appearance finished_appearance
    ) {
        ISubfigure started_subfigure = null;
        appearance_to_subfigure.TryGetValue(
            finished_appearance, out started_subfigure
        );
        if (started_subfigure != null) {
            finished_subfigures.Add(started_subfigure);
        }
    }

    // it's ok to have ALL links stored (even through the nodes)
    // img: pruned vs unpruned connections in Figures
    public static IFigure prune_exessive_links(IFigure figure) {
        return figure;
    }
}
}