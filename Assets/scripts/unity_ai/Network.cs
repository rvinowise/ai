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
    [SerializeField] private Figure figure_preafab;
    [SerializeField] private ai.unity.Action_history action_history;
    [SerializeField] private ai.unity.Figure_showcase figure_table;
    [SerializeField] private Selector selector;
    
    private ISequence_finder sequence_finder;
    private IFigure_provider figure_provider;
    private Figure_builder_from_signals figure_builder_from_signals;
    
    private ai.simple.Network network;

    void Awake() {
        network = new ai.simple.Network(
            action_history,
            sequence_finder
        );
        init_modules();
        init_network();
    }

    private void init_modules() {
        figure_provider = new Figure_provider(figure_preafab);
        sequence_finder = new Sequence_finder(
            action_history,
            figure_provider
        );
        figure_builder_from_signals = new Figure_builder_from_signals(figure_provider);
    }

    private void init_network() {
        Network_initialiser network_initialiser = new Network_initialiser(
            figure_provider
        );
        network_initialiser.create_base_signals();
    }


    [called_by_unity]
    public void on_create_figure_from_actions() {
        var selected_groups = selector.sorted_action_groups;
        figure_builder_from_signals.create_figure_from_action_history(selected_groups);
    }
}
}