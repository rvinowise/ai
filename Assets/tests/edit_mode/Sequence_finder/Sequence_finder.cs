using UnityEngine;
using NUnit.Framework;
using rvinowise.ai.general;

using rvinowise.ai.simple;

using Network = rvinowise.ai.simple.Network;


namespace rvinowise.ai.unit_tests.sequence_finder {


[TestFixture]
public partial class two_signals_repeat_twice
{
    private INetwork<Figure> network;
    private ISequence_finder<Figure> sequence_finder;
    private IFigure_provider<Figure> figure_provider;
    public two_signals_repeat_twice() {
        
    }

    private void fill_action_history_with_inputs(string[] raw_input) {
        foreach (var signal in raw_input) {
            network.input_signal(signal);
        }
    }

    [SetUp]
    public void prepare_network() {
        network = Network.get_network_with_base_signals();
        sequence_finder = network.sequence_finder;
        figure_provider = network.figure_provider;
    }
    
    [Test]
    [TestCaseSource(nameof(sequence_cases))]
    public void repeated_sequences_are_found(
        string[] input,
        int[][] result
    ) {
        fill_action_history_with_inputs(input);
        sequence_finder.enrich_storage_with_sequences();
        IFigure found_sequence = figure_provider.find_figure_with_id("01");
        Assert.IsNotNull(found_sequence);
        verify_figure_appearances(found_sequence, result);
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