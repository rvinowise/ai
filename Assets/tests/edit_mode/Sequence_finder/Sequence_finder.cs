using NUnit.Framework;
using rvinowise.ai.general;
using rvinowise.ai.simple;
using rvinowise.ai.unity;
using UnityEngine;
using Network_initialiser = rvinowise.ai.simple.Network_initialiser;

namespace rvinowise.ai.unit_tests.sequence_finder {

public class Network {

    public readonly ISequence_finder sequence_finder;

    private readonly IAction_history action_history =
        new simple.Action_history();
    
    public readonly IFigure_provider figure_provider;

    public Network() {
        figure_provider = new Figure_provider(create_simple_figure);
        sequence_finder = new Sequence_finder(
            action_history,
            figure_provider
        );
        fill_figure_storage_with_base_signals();
    }

    public IFigure create_simple_figure(string id) {
        return new ai.simple.Figure(id);
    }

    public void input_signal(string id) {
        action_history.input_signals(
            new[] {figure_provider.find_figure_with_id(id)}    
        );
    }

    private void fill_figure_storage_with_base_signals() {
        INetwork_initialiser network_initialiser = 
            new Network_initialiser(figure_provider);
        network_initialiser.create_base_signals();
    }

 

}

[TestFixture]
public class Testing_sequence_finder {
    Network network;
}

[TestFixture]
public partial class two_signals_repeat_twice
{
    private readonly Network network = new Network();
    public two_signals_repeat_twice() {
        fill_action_history_with_inputs();
    }

    private void fill_action_history_with_inputs() {
        foreach (var signal in raw_input) {
            network.input_signal(signal);
        }
    }
    
    [Test]
    public void repeated_sequences_are_found() {
        network.sequence_finder.enrich_storage_with_sequences();
        IFigure found_sequence = network.figure_provider.find_figure_with_id("01");
        Assert.IsNotNull(found_sequence);
        verify_figure_appearances(found_sequence, expected_appearances);
    }

    private void verify_figure_appearances(
        IFigure tested_figure,
        int[][]expected_appearances
    ) {
        var tested_appearances = tested_figure.get_appearances();
        Assert.AreEqual(tested_appearances.Count, expected_appearances.Length);
        for(
            int i_appearance=0;
            i_appearance < expected_appearances.Length;
            i_appearance++
        ) {
            Assert.AreEqual(
                tested_appearances[i_appearance].get_start().action_group.moment,
                expected_appearances[i_appearance][0]
            );
            Assert.AreEqual(
                tested_appearances[i_appearance].get_end().action_group.moment,
                expected_appearances[i_appearance][1]
            );
        }
    }

}


}