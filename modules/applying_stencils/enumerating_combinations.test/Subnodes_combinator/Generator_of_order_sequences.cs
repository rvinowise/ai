using NUnit.Framework;
using rvinowise.ai.mapping_stencils;

namespace rvinowise.ai.unit_tests.generator_of_order_sequences {

[TestFixture]
public partial class initialised_correctly_for_generation {
    
    private Generator_of_order_sequences<int[]> init_generator() {
        Generator_of_order_sequences<int[]> generator = 
            new Generator_of_order_sequences<int[]>();

        generator.add_order(new Generator_of_mappings(4, 2));
        generator.add_order(new Generator_of_mappings(3, 3));

        return generator;
    }

    [Test]
    public void all_combinations_are_provided_in_a_loop() {
        var generator = init_generator();

        int i_combination = 0;
        foreach (var combination in generator) {

            Assert.That(
                result_total_combinations[i_combination],
                Is.EqualTo(combination),
                $"combination # {i_combination}"
            );
            i_combination++;
        }
        
    }
}



}