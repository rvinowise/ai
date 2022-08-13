using UnityEngine;
using rvinowise.ai.general;
using rvinowise.ai.simple;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.ui.input;



namespace rvinowise.ai.unity {


/* used for assigning prefabs to the modules of the network */
public class Network:
MonoBehaviour 
{
    [SerializeField] public ai.unity.Action_history action_history;
    [SerializeField] public Figure_showcase figure_showcase;
    
    internal ISequence_finder<Figure> sequence_finder;
    private Figure_builder_from_signals figure_builder_from_signals;
    

    void Awake() {
        init_modules();
        init_interface();
    }

    void Start() {
        init_network();
    }
    

    private void init_modules() {
        sequence_finder = new Sequence_finder<Figure>(
            action_history,
            figure_showcase
        );
        figure_builder_from_signals = new Figure_builder_from_signals(figure_showcase);
    }

    private void init_network() {
        new Base_signals<Figure>(
            figure_showcase
        );
    }

    private void init_interface() {
        //selector.figure_provider = figure_showcase;
    }

    [called_by_unity]
    public void on_create_figure_from_actions() {
        var selected_groups = Sele.get_selected_action_groups();
        figure_builder_from_signals.create_figure_from_action_history(selected_groups);
    }
}
}