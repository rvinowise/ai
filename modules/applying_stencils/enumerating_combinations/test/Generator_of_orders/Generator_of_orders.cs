using NUnit.Framework;
using rvinowise.ai.mapping_stencils;
using rvinowise.contracts;

namespace rvinowise.ai.test.generator_of_order_sequences {

[TestFixture]
public partial class initialised_correctly_for_generation {
    
    private Generator_of_orders<int[]> get_generator() {
        return new Generator_of_orders<int[]>();
    }

    [Test]
    public void all_combinations_are_provided_in_a_loop() {
        var generator = get_generator();

        generator.add_order(new Generator_of_mappings(2, 4));
        generator.add_order(new Generator_of_mappings(3, 3));
        
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
    
    [Test]
    public void order_with_only_one_taken_is_possible() {
        var generator = get_generator();

        generator.add_order(new Generator_of_mappings(1, 3));
        generator.add_order(new Generator_of_mappings(1, 1));
        
        int i_combination = 0;
        foreach (var combination in generator) {
            
            i_combination++;
        }
        
    }

    [Test]
    public void only_one_order_is_possible() {
        var generator = get_generator();

        generator.add_order(new Generator_of_mappings(1, 3));

        var expected = new[] {
            new[] {
                new[] { 0 }
            },
            new[] {
                new[] { 1 }
            },
            new[] {
                new[] { 2 }
            }
        };

        int i_combination = 0;
        foreach (var combination in generator) {
            Assert.That(
                expected[i_combination],
                Is.EqualTo(combination)
            );
            i_combination++;
        }
        
    }

    [Test]
    public void manual_iteration_with_IEnumerable_is_possible() {
        var generator = get_generator();

        generator.add_order(new Generator_of_mappings(1, 3));

        var enumerator = generator.GetEnumerator();
        var test0 = enumerator.Current;
        var test1 = enumerator.MoveNext();
        var test2 = enumerator.Current;
        var test3 = enumerator.MoveNext();
        var test4 = enumerator.Current;
        var test5 = enumerator.MoveNext();
        var test6 = enumerator.Current;
        var test7 = enumerator.MoveNext();

    }
    
    [Test]
    public void throws_if_enumerated_without_added_orders() {
        var generator = get_generator();

        var enumerator = generator.GetEnumerator();
        
        Assert.Throws<Broken_contract_exception>(
            () => enumerator.MoveNext()
        );


    }
}



}