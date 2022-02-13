using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using rvinowise.ai.general;
using rvinowise.ai.unity.mapping_stencils;
using UnityEngine;
using UnityEngine.TestTools;

namespace tests {


public partial class Stencil_application_mapping_combinator {
    private const int max_subnodes = 5;
    private const int needed_amount = 3;
    private const int combinations_n = 0;
    
    
    [Test]
    public void loop_over_combinations_for_figure() {
        Combination_for_figure combination = new Combination_for_figure(
            max_subnodes, needed_amount
        );
        int i_combination = 0;
        while (combination.MoveNext()) {
            Assert.Equals(
                combination.combination,
                result_combinations[i_combination++]
            );
        }

        Assert.Equals(i_combination, result_combinations.Length);
    }

    


    private Figure_combinator_figure_requirement[] combinator_figure_requirements = {
        new Figure_combinator_figure_requirement(0, 2, 4),
        new Figure_combinator_figure_requirement(1, 3, 3),
    };
    
    
    [Test]
    public void loop_over_subnodes_combinations() {
        Subnodes_combination combination = new Subnodes_combination(
            combinator_figure_requirements
        );

        int i_combination = 0;
        while (combination.MoveNext()) {
            Assert.Equals(combination. result_total_combinations);
            i_combination++;
        }
        
    }
}


}