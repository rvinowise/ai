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

[TestFixture]
public partial class Sequences_can_be_found {
    
    private ISequence_finder sequence_finder;
    
    private IAction_history action_history;
    
    private IFigure_storage figure_storage;


    [SetUp]
    public void setup() {
        fill_action_history();
        prepare_sequence_finder();
    }

    private void prepare_sequence_finder() {
        sequence_finder = new GameObject().AddComponent<Sequence_finder>();
        ISequence_builder sequence_builder = new ai.simple.Sequence_builder(figure_storage);
        sequence_finder.init_unity_fields(
            action_history,
            figure_storage,
            sequence_builder
        );
    }

    private void fill_action_history() {
        fill_figure_storage_with_base_signals();
        action_history = new simple.Action_history();
        
        for(int i=0;i<max_groups;i++) {
            action_history.create_next_action_group(0);
        }
        history.create_figure_appearance()
    }

    private void fill_figure_storage_with_base_signals() {
        figure_storage = new ai.simple.Figure_storage();
        
        INetwork_initialiser network_initialiser = new Network_initialiser(figure_storage);
        network_initialiser.create_base_signals();
    }

   
    
    [Test]
    public void find_repeated_pairs() {
        sequence_finder = new GameObject().AddComponent<Sequence_finder>();
        
    }

}


}