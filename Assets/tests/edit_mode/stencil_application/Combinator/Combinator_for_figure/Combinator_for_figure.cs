using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using rvinowise.ai.general;
using rvinowise.ai.unity.mapping_stencils;
using UnityEngine;
using UnityEngine.TestTools;

namespace rvinowise.ai.unit_tests.combinator_for_figure {

[TestFixture]
public partial class regular_loop_over_combinations_can_be_done {
    private const int max_subnodes = 5;
    private const int needed_amount = 3;
    
    [Test]
    public void all_combinations_are_provided_in_a_loop() {
        Combinator_for_figure combinator = new Combinator_for_figure(
            max_subnodes, needed_amount
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

[TestFixture]
public class not_enough_occurances_of_figure {
    private const int max_subnodes = 5;
    private const int needed_amount = 6;
    
    [Test]
    public void zero_iterations_are_possible() {
        Combinator_for_figure combinator = new Combinator_for_figure(
            max_subnodes, needed_amount
        );
        Assert.IsFalse(combinator.MoveNext());
    }


}


}