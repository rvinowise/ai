using NUnit.Framework;
using rvinowise.ai.general;
using rvinowise.ai.simple;
using rvinowise.ai.unity;
using UnityEngine;
using Network = rvinowise.ai.simple.Network;

namespace rvinowise.ai.unit_tests.sequence_finder {



[TestFixture]
public class Testing_sequence_finder {
    Network network;
}

[TestFixture]
public partial class two_signals_repeat_twice
{
    private readonly INetwork network = Network.get_network_with_base_signals();
    private readonly ISequence_finder sequence_finder;
    private readonly IFigure_provider figure_provider;
    public two_signals_repeat_twice() {
        sequence_finder = network.get_sequence_finder();
        figure_provider = network.get_figure_provider();
        fill_action_history_with_inputs();
    }

    private void fill_action_history_with_inputs() {
        foreach (var signal in raw_input) {
            network.input_signal(signal);
        }
    }
    
    [Test]
    public void repeated_sequences_are_found() {
        sequence_finder.enrich_storage_with_sequences();
        IFigure found_sequence = figure_provider.find_figure_with_id("01");
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
                (int)tested_appearances[i_appearance].get_start().action_group.moment,
                expected_appearances[i_appearance][0]
            );
            Assert.AreEqual(
                (int)tested_appearances[i_appearance].get_end().action_group.moment,
                expected_appearances[i_appearance][1]
            );
        }
    }

}


}