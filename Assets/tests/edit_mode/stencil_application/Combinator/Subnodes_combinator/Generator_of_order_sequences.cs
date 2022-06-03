using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Internal;
using rvinowise.ai.simple.mapping_stencils;
using System.Linq;

namespace rvinowise.ai.unit_tests.generator_of_order_sequences {

[TestFixture]
public partial class initialised_correctly_for_generation {
    
    private Generator_of_order_sequences<int[]> init_generator() {
        Generator_of_order_sequences<int[]> generator = 
            new Generator_of_order_sequences<int[]>();

        generator.add_order(new Generator_of_mappings(2, 4));
        generator.add_order(new Generator_of_mappings(3, 3));

        return generator;
    }

    [Test]
    public void all_combinations_are_provided_in_a_loop() {
        var generator = init_generator();

        int i_combination = 0;
        foreach (var combination in generator) {

            Assert.AreEqual(
                result_total_combinations[i_combination],
                combination,
                $"combination # {i_combination}"
            );
            i_combination++;
        }
        
    }
}

[TestFixture]
public class not_enough_occurances_of_first_figure {

    private Generator_of_order_sequences<int[]> init_generator() {
        Generator_of_order_sequences<int[]> generator = 
            new Generator_of_order_sequences<int[]>();

        generator.add_order(new Generator_of_mappings(5, 4));
        generator.add_order(new Generator_of_mappings(3, 3));

        return generator;
    }
    
    [Test]
    public void zero_cycle_interations_are_possible() {
        Generator_of_order_sequences<int[]> generator = 
            init_generator();

        Assert.IsEmpty(generator);
    }
}

[TestFixture]
public class not_enough_occurances_of_second_figure {
    private Generator_of_order_sequences<int[]> init_generator() {
        Generator_of_order_sequences<int[]> generator = 
            new Generator_of_order_sequences<int[]>();

        generator.add_order(new Generator_of_mappings(3, 4));
        generator.add_order(new Generator_of_mappings(5, 3));

        return generator;
    }
    
    [Test]
    public void zero_cycle_interations_are_possible() {
        Generator_of_order_sequences<int[]> generator = 
           init_generator();

        Assert.That(
            generator,
            Is.Empty
        );

    }
}

}