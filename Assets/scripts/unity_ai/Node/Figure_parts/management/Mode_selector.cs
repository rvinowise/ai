using System.Collections.Generic;
using UnityEngine;
using rvinowise.ai.unity;
using Action = rvinowise.ai.unity.Action;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;


namespace rvinowise.unity.ui.input {

public class Mode_selector:
    MonoBehaviour 
{
    [SerializeField] private Manual_figure_builder manual_figure_builder;
    [SerializeField] private Figure_observer figure_observer;
    public static Mode_selector instance;
    void Awake() {
        Contract.Requires(instance==null, "singleton");
        instance = this;
        manual_figure_builder.mode_selector = this;
        //figure_observer.mode_selector = this;

        manual_figure_builder.deactivate();
        manual_figure_builder.figure_observer = figure_observer;
        figure_observer.activate();
    }
    
    public void on_start_building_figure() {
        figure_observer.deactivate();
        manual_figure_builder.on_create_empty_figure();
    }
    public void on_start_editing_figure() {
        if (figure_observer.enabled) {
            Figure observed_figure = figure_observer.observed_figure;
            figure_observer.deactivate();
            manual_figure_builder.on_start_editing_figure(observed_figure);
        }
    }public void on_start_editing_figure_without_changing_connections() {
        if (figure_observer.enabled) {
            Figure observed_figure = figure_observer.observed_figure;
            figure_observer.deactivate();
            manual_figure_builder.on_start_editing_figure(observed_figure);
        }
    }
    
    public void on_finish_building_figure() {
        manual_figure_builder.deactivate();
        figure_observer.activate();
    }

    public void stop_observing_figure() {
        figure_observer.finish_observing();
    }


}
}