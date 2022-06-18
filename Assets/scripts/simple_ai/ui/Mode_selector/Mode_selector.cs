// using System.Collections.Generic;
// using UnityEngine;
// using Action = rvinowise.ai.unity.Action;
// using rvinowise.ai.general;
// using rvinowise.ai.ui.general;
// using rvinowise.rvi.contracts;
//
//
//
// namespace rvinowise.ai.ui.simple {
//
//
// public class Mode_selector:
//     IMode_selector
// {
//     private IManual_figure_builder manual_figure_builder;
//     private IFigure_observer figure_observer;
//
//     public Mode_selector(
//         IManual_figure_builder manual_figure_builder,
//         IFigure_observer figure_observer
//     ) {
//         this.manual_figure_builder = manual_figure_builder;
//         this.figure_observer = figure_observer;
//     }
//     
//     public Mode_selector() {
//         manual_figure_builder.deactivate();
//     }
//     
//     public void on_start_building_figure() {
//         figure_observer.finish_observing();
//         manual_figure_builder.on_create_empty_figure();
//     }
//     public void on_start_editing_figure() {
//         if (figure_observer.observed_figure is IFigure observed_figure) {
//             figure_observer.finish_observing();
//             manual_figure_builder.on_start_editing_figure(observed_figure);
//         }
//     }
//     
//     public void on_start_editing_figure_without_changing_connections() {
//         if (figure_observer.observed_figure is IFigure observed_figure) {
//             figure_observer.finish_observing();
//             manual_figure_builder.change_connections = true;
//             manual_figure_builder.on_start_editing_figure(observed_figure);
//         }
//     }
//     
//     public void on_finish_building_figure() {
//         manual_figure_builder.deactivate();
//         figure_observer.observe(manual_figure_builder.figure);
//     }
//
//     public void stop_observing_figure() {
//         figure_observer.finish_observing();
//     }
//
//
// }
// }