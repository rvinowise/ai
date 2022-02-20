using System.Collections;
using System.Collections.Generic;
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
    private const int max_subnodes = 5;
    private const int needed_amount = 3;
    
    [Test]
    public void find_repeated_pairs() {
        Sequence_finder sequence_finder = new Sequence_finder(
            
        );
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