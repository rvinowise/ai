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
    
    
    [Test]
    public void find_repeated_pairs() {
        Sequence_finder sequence_finder = new GameObject().AddComponent<Sequence_finder>();

        var action_history = new Mock<IAction_history>();
        
        sequence_finder.action_history = action_history;
        sequence_finder.figure_storage = figure_storage;
        sequence_finder.sequence_builder = sequence_builder;
        
        sequence_finder.enrich_storage_with_sequences();
        
        int i_combination = 0;
        while (combinator.MoveNext()) {
            Debug.Log(
                string.Join(", ", combinator.combination)
            );
            CollectionAssert.AreEquivalent(
                result_combinations[i_combination++],
                combinator.combination
            );
        }

        Assert.AreEqual(result_combinations.Length, i_combination);
    }
    
    [Test]
    public void find_repeated_pairs_0() {
        Sequence_finder sequence_finder = new GameObject().AddComponent<Sequence_finder>();
        Action_history action_history = new GameObject().AddComponent<Action_history>();
        Figure_storage figure_storage = new GameObject().AddComponent<Figure_storage>();
        Sequence_builder sequence_builder = new GameObject().AddComponent<Sequence_builder>();

        sequence_finder.action_history = action_history;
        sequence_finder.figure_storage = figure_storage;
        sequence_finder.sequence_builder = sequence_builder;
        
        sequence_finder.enrich_storage_with_sequences();
        
        int i_combination = 0;
        while (combinator.MoveNext()) {
            Debug.Log(
                string.Join(", ", combinator.combination)
            );
            CollectionAssert.AreEquivalent(
                result_combinations[i_combination++],
                combinator.combination
            );
        }

        Assert.AreEqual(result_combinations.Length, i_combination);
    }
}


}