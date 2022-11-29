using System;
using NUnit.Framework;
using rvinowise.ai.mapping_stencils;

namespace rvinowise.ai.unit_tests.generator_of_mappings {

[TestFixture]
public partial class regular_loop_over_combinations_can_be_done {
    private const int max_subnodes = 5;
    private const int needed_amount = 3;
    
    [Test]
    public void all_combinations_are_provided_in_a_loop() {
        Generator_of_mappings generator = new Generator_of_mappings(
            max_subnodes, needed_amount
        );
        int i_combination = 0;
        foreach (int[] combination in generator) {
            // Debug.Log(
                // string.Join(", ", combination)
            // );
            CollectionAssert.AreEquivalent(
                result_combinations[i_combination++],
                combination
            );
        }

        Assert.That(result_combinations.Length, Is.EqualTo(i_combination));
    }
}

[TestFixture]
public class not_enough_occurances_of_figure {
    private const int max_subnodes = 2;
    private const int needed_amount = 6;
    
    [Test]
    public void zero_iterations_are_possible() {
        Assert.Throws<ArgumentException>(
            () => new Generator_of_mappings(
                max_subnodes, needed_amount
            )
        );
        //Assert.IsEmpty(generator);
    }


}


}