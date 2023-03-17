using System;
using NUnit.Framework;
using rvinowise.ai.mapping_stencils;

namespace rvinowise.ai.test.generator_of_mappings {

[TestFixture]
public partial class regular_loop_over_combinations_can_be_done {
    private const int taken_amount = 3;
    private const int available_amount = 5;
    
    [Test]
    public void all_combinations_are_provided_in_a_loop() {
        Generator_of_mappings generator = new Generator_of_mappings(
            taken_amount, available_amount 
        );
        int i_combination = 0;
        foreach (int[] combination in generator) {
            CollectionAssert.AreEquivalent(
                result_combinations[i_combination++],
                combination
            );
        }

        Assert.That(result_combinations.Length, Is.EqualTo(i_combination));
    }
}

[TestFixture]
public class not_enough_occurences_of_figure {
    private const int taken_amount = 6;
    private const int available_amount = 2;
    
    [Test]
    public void zero_iterations_are_possible() {
        Assert.Throws<ArgumentException>(
            () => new Generator_of_mappings(
                taken_amount, available_amount 
            )
        );
        //Assert.IsEmpty(generator);
    }
}



}