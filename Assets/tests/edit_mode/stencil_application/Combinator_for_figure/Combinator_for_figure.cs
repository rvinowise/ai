using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using rvinowise.ai.general;
using rvinowise.ai.unity.mapping_stencils;
using UnityEngine;
using UnityEngine.TestTools;

namespace rvinowise.unit_tests.combinator_for_figure {

[TestFixture]
public partial class regular_loop_over_combinations {
    private const int max_subnodes = 5;
    private const int needed_amount = 3;
    
    
    [Test]
    public void loop_over_combinations_for_figure() {
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
public class not_enough_occurances {
    private const int max_subnodes = 5;
    private const int needed_amount = 6;
    
    
    
    [Test]
    public void no_result_is_given() {
        Combinator_for_figure combinator = new Combinator_for_figure(
            max_subnodes, needed_amount
        );
        Assert.IsFalse(combinator.MoveNext());
    }


}


}