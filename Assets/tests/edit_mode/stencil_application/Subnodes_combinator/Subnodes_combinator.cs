using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using rvinowise.ai.general;
using rvinowise.ai.unity.mapping_stencils;
using UnityEngine;
using UnityEngine.TestTools;

namespace rvinowise.unit_tests.subnodes_combinator {

[TestFixture]
public partial class regular_loop_over_combinations {

    private Figure_combinator_figure_requirement[] combinator_figure_requirements = {
        new Figure_combinator_figure_requirement(2, 4),
        new Figure_combinator_figure_requirement(3, 3),
    };
    
    [Test]
    public void loop_over_subnodes_combinations() {
        Subnodes_combinator combinator = new Subnodes_combinator(
            combinator_figure_requirements
        );

        int i_combination = 0;
        while (combinator.MoveNext()) {
            Assert.AreEqual(
                result_total_combinations[i_combination++],
                combinator.get_combination_as_indexes()
            );
        }
        
    }
}

[TestFixture]
public class not_enough_occurances_of_first_figure {

    private Figure_combinator_figure_requirement[] combinator_figure_requirements = {
        new Figure_combinator_figure_requirement(5, 4),
        new Figure_combinator_figure_requirement(3, 3),
    };
    
    [Test]
    public void zero_cycle_interations_are_possible() {
        Subnodes_combinator combinator = new Subnodes_combinator(
            combinator_figure_requirements
        );

        Assert.IsFalse(combinator.MoveNext());

    }
}

[TestFixture]
public class not_enough_occurances_of_second_figure {

    private Figure_combinator_figure_requirement[] combinator_figure_requirements = {
        new Figure_combinator_figure_requirement(3, 4),
        new Figure_combinator_figure_requirement(5, 3),
    };
    
    [Test]
    public void zero_cycle_interations_are_possible() {
        Subnodes_combinator combinator = new Subnodes_combinator(
            combinator_figure_requirements
        );

        Assert.That(
            combinator.MoveNext(),
            Is.False
        );

    }
}

}