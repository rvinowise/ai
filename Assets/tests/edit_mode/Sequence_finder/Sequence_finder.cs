using System.Collections;
using System.Collections.Generic;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Internal;
using rvinowise.ai.general;
using rvinowise.ai.unity;
using rvinowise.ai.unity.mapping_stencils;
using UnityEngine;
using UnityEngine.TestTools;
using Action_history = rvinowise.ai.unity.Action_history;
using Figure_storage = rvinowise.ai.unity.Figure_storage;
using Network_initialiser = rvinowise.ai.simple.Network_initialiser;

namespace rvinowise.ai.unit_tests.sequence_finder {

public class Network {

    public readonly ISequence_finder sequence_finder =
        new GameObject().AddComponent<Sequence_finder>();

    private readonly IAction_history action_history =
        new simple.Action_history();
    
    public readonly IFigure_storage figure_storage =
        new ai.simple.Figure_storage();

    public Network() {
        fill_figure_storage_with_base_signals();
        init_sequence_finder();
    }

    public void input_signal(string id) {
        action_history.input_signals(
            new[] {figure_storage.find_figure_with_id(id)}    
        );
    }

    private void fill_figure_storage_with_base_signals() {
        INetwork_initialiser network_initialiser = 
            new Network_initialiser(figure_storage);
        network_initialiser.create_base_signals();
    }

    private void init_sequence_finder() {
        ISequence_builder sequence_builder = new ai.simple.Sequence_builder(
            figure_storage
        );
        sequence_finder.init_unity_fields(
            action_history,
            figure_storage,
            sequence_builder
        );
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
        IFigure found_sequence = network.figure_storage.find_figure_with_id("01");
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