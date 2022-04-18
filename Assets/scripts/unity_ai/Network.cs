using UnityEngine;
using System;
using rvinowise.ai.general;
using rvinowise.ai.simple;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.ui.input;

namespace rvinowise.ai.unity {


/* used for assigning prefabs to the modules of the network */
public class Network:
MonoBehaviour {
    [SerializeField] private ai.unity.Action_history action_history;
    [SerializeField] private Figure_showcase figure_showcase;
    [SerializeField] private Selector selector;
    
    internal ISequence_finder sequence_finder;
    private Figure_builder_from_signals figure_builder_from_signals;
    
    private ai.simple.Network network;

    void Awake() {
        network = new ai.simple.Network(
            action_history,
            sequence_finder
        );
        init_modules();
        init_network();
        init_interface();
    }

    private void init_modules() {
        figure_showcase.init();
        sequence_finder = new Sequence_finder(
            action_history,
            figure_showcase
        );
        figure_builder_from_signals = new Figure_builder_from_signals(figure_showcase);
    }

    private void init_network() {
        Figure_provider_initialiser figure_provider_initialiser = new Figure_provider_initialiser(
            figure_showcase
        );
        figure_provider_initialiser.create_base_signals();
    }

    private void init_interface() {
        selector.figure_provider = figure_showcase;
    }

    [called_by_unity]
    public void on_create_figure_from_actions() {
        var selected_groups = selector.sorted_action_groups;
        figure_builder_from_signals.create_figure_from_action_history(selected_groups);
    }
}
}