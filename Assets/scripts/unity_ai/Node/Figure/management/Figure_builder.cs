
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;

namespace rvinowise.ai.unity {

public class Figure_builder: MonoBehaviour {

    public Action_history action_history;
    public Figure_storage figure_storage;
    private Figure figure_prefab;
    
    private Figure figure; //which is being built by this builder
    private Figure_representation representation; //which is being built by this builder
    private List<ISubfigure> all_subfigures = new List<ISubfigure>();
    private List<ISubfigure> ended_subfigures = new List<ISubfigure>();
    
    private Dictionary<IFigure_appearance, ISubfigure> 
    appearance_to_subfigure 
    = new Dictionary<IFigure_appearance, ISubfigure>();

    private int last_subfigure_id;

    void Awake() {
        figure_prefab = figure_storage.figure_prefab;
    }
    
    public void on_create_figure_from_actions() {
        var selected_groups = Selection.instance.sorted_action_groups;
        create_figure_from_action_history(selected_groups);
    }

    public IFigure create_figure_from_action_history(
        IReadOnlyList<IAction_group> action_groups
    ) {
        clear();
        figure = figure_storage.add_new_figure() as Figure;
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
        Subfigure new_subfigure = representation.add_subfigure(appended_figure.figure);
        new_subfigure.id = (last_subfigure_id++).ToString();
        appearance_to_subfigure.Add(appended_figure, new_subfigure);
        if (ended_subfigures.Any()) {
            foreach (ISubfigure ended_subfigure in ended_subfigures) {
                ended_subfigure.connext_to_next(new_subfigure);
            }
        }
        else {
            representation.first_subfigures.Add(new_subfigure);
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
            ended_subfigures.Add(started_subfigure);
        }
    }

    // it's ok to have ALL links stored (even through the nodes)
    // img: pruned vs unpruned connections in Figures
    public static IFigure prune_exessive_links(IFigure figure) {
        return figure;
    }
}
}