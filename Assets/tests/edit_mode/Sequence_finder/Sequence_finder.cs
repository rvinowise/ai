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

namespace rvinowise.unit_tests.sequence_finder {

[TestFixture]
public partial class sequences_can_be_found {
    
    private Mock<Action_history> action_history_mock;
    private IReadOnlyList<Mock<IAction_group>> raw_action_groups;
    
    [SetUp]
    public void prepare_raw_signals() {
        raw_action_groups = prepare_action_groups();
        
        action_history_mock = new Mock<rvinowise.ai.unity.Action_history>();
        action_history_mock.Setup(h => h.get_action_groups(0,10)).Returns(
            raw_action_groups
        );
        
    }

    const int max_groups = 10;
    private IReadOnlyList<Mock<IAction_group>> prepare_action_groups() {
        List<Mock<IAction_group>> groups = new List<Mock<IAction_group>>();
        for (int i_group = 0;i_group < max_groups; i_group++) {
            groups.Add(new Mock<IAction_group>());
        }
        groups[0].Object.add_action(new Mock<IAppearance_start>().Setup(a=>
            a.figure.));
        
        return groups.AsReadOnly();
    }
    
    [Test]
    public void find_repeated_pairs() {
        
        
        Sequence_finder sequence_finder = new GameObject().AddComponent<Sequence_finder>();
        sequence_finder.action_history = action_history_mock.Object;
        
        sequence_finder.enrich_storage_with_sequences();
        
        Debug.Log("random gave: ");

        // Sequence_finder sequence_finder = new GameObject().AddComponent<Sequence_finder>();
        //
        // var action_history = new Mock<IAction_history>();
        //
        // sequence_finder.action_history = action_history;
        // sequence_finder.figure_storage = figure_storage;
        // sequence_finder.sequence_builder = sequence_builder;
        //
        // sequence_finder.enrich_storage_with_sequences();
        //
        // int i_combination = 0;
        // while (combinator.MoveNext()) {
        //     Debug.Log(
        //         string.Join(", ", combinator.combination)
        //     );
        //     CollectionAssert.AreEquivalent(
        //         result_combinations[i_combination++],
        //         combinator.combination
        //     );
        // }
        //
        // Assert.AreEqual(result_combinations.Length, i_combination);
    }
    
    [Test]
    public void find_repeated_pairs_0() {
        // Sequence_finder sequence_finder = new GameObject().AddComponent<Sequence_finder>();
        // Action_history action_history = new GameObject().AddComponent<Action_history>();
        // Figure_storage figure_storage = new GameObject().AddComponent<Figure_storage>();
        // Sequence_builder sequence_builder = new GameObject().AddComponent<Sequence_builder>();
        //
        // sequence_finder.action_history = action_history;
        // sequence_finder.figure_storage = figure_storage;
        // sequence_finder.sequence_builder = sequence_builder;
        //
        // sequence_finder.enrich_storage_with_sequences();
        //
        // int i_combination = 0;
        // while (combinator.MoveNext()) {
        //     Debug.Log(
        //         string.Join(", ", combinator.combination)
        //     );
        //     CollectionAssert.AreEquivalent(
        //         result_combinations[i_combination++],
        //         combinator.combination
        //     );
        // }
        //
        // Assert.AreEqual(result_combinations.Length, i_combination);
    }
}


}