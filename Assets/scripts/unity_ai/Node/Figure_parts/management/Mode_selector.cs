using System.Collections.Generic;
using UnityEngine;
using rvinowise.ai.unity;
using Action = rvinowise.ai.unity.Action;
using rvinowise.ai.general;


namespace rvinowise.unity.ui.input {

public class Mode_selector : MonoBehaviour {
    [SerializeField] private Manual_figure_builder manual_figure_builder;
    [SerializeField] private Figure_observer figure_observer;

    void Awake() {
        manual_figure_builder.mode_selector = this;
        //figure_observer.mode_selector = this;

        manual_figure_builder.deactivate();
        figure_observer.activate();
    }
    
    public void on_start_building_figure() {
        manual_figure_builder.activate();
        figure_observer.deactivate();
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