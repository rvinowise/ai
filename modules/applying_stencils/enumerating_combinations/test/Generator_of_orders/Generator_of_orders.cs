using NUnit.Framework;
using rvinowise.ai.mapping_stencils;
using rvinowise.contracts;

namespace rvinowise.ai.test.generator_of_order_sequences {

[TestFixture]
public partial class filling_generator_or_orders_with_orders {
    
    
    [Test]
    public void all_combinations_are_provided_in_a_loop() {
        var generator = new Generator_of_orders<int[]>(
            new List<Generator_of_mappings>{
                new(2, 4),
                new(3, 3)
            }
        );

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
        var generator = new Generator_of_orders<int[]>(
            new List<Generator_of_mappings>{
                new(1, 3),
                new(1, 1)
            }
        );

        
        int i_combination = 0;
        foreach (var combination in generator) {
            
            i_combination++;
        }
        
    }

    [Test]
    public void only_one_order_is_possible() {
        var generator = new Generator_of_orders<int[]>(
            new List<Generator_of_mappings>{
                new(1, 3)
            }
        );

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
        var generator = new Generator_of_orders<int[]>(
            new List<Generator_of_mappings>{
                new(1, 3)
            }
        );

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
    public void using_with_generalised_orders_which_return_objects() {

        var generator = new Generator_of_orders<IEnumerable<Element_to_target<string,string>>>(
            new List<Generator_of_individualised_mappings<string, string>>{
                new(
                    new List<Element_to_targets<string, string>> {
                        new("A1", new List<string>{"a1","a2","a3","a4"}),
                        new("A2", new List<string>{"a1","a2","a3"}),
                        new("A3", new List<string>{"a1","a2"}),
                    }
                ),
                new(
                    new List<Element_to_targets<string, string>> {
                        new("B1", new List<string>{"b1","b2"}),
                        new("B2", new List<string>{"b1","b2"})
                    }
                )
            }
        );
        
    //  states      figures     elements
        IEnumerable<IEnumerable<IEnumerable<Element_to_target<string, string>>>> result_combinations = 
            new List<List<List<Element_to_target<string, string>>>> {
                //1st iteration of the 2nd order
                new(){
                    new List<Element_to_target<string, string>> {new("A1","a3"),new("A2","a2"),new("A3","a1") },
                    new List<Element_to_target<string, string>> {new("B1","b2"),new("B2","b1") }
                },
                new(){
                    new List<Element_to_target<string, string>> {new("A1","a4"),new("A2","a2"),new("A3","a1") },
                    new List<Element_to_target<string, string>> {new("B1","b2"),new("B2","b1") }
                },
                new(){
                    new List<Element_to_target<string, string>> {new("A1","a2"),new("A2","a3"),new("A3","a1") },
                    new List<Element_to_target<string, string>> {new("B1","b2"),new("B2","b1") }
                },
                new(){
                    new List<Element_to_target<string, string>> {new("A1","a4"),new("A2","a3"),new("A3","a1") },
                    new List<Element_to_target<string, string>> {new("B1","b2"),new("B2","b1") }
                },
                new(){
                    new List<Element_to_target<string, string>> {new("A1","a3"),new("A2","a1"),new("A3","a2") },
                    new List<Element_to_target<string, string>> {new("B1","b2"),new("B2","b1") }
                },
                new(){
                    new List<Element_to_target<string, string>> {new("A1","a4"),new("A2","a1"),new("A3","a2") },
                    new List<Element_to_target<string, string>> {new("B1","b2"),new("B2","b1") }
                },
                new(){
                    new List<Element_to_target<string, string>> {new("A1","a1"),new("A2","a3"),new("A3","a2") },
                    new List<Element_to_target<string, string>> {new("B1","b2"),new("B2","b1") }
                },
                new(){
                    new List<Element_to_target<string, string>> {new("A1","a4"),new("A2","a3"),new("A3","a2") },
                    new List<Element_to_target<string, string>> {new("B1","b2"),new("B2","b1") }
                },
                //2nd iteration of the 2nd order
                new(){
                    new List<Element_to_target<string, string>> {new("A1","a3"),new("A2","a2"),new("A3","a1") },
                    new List<Element_to_target<string, string>> {new("B1","b1"),new("B2","b2") }
                },
                new(){
                    new List<Element_to_target<string, string>> {new("A1","a4"),new("A2","a2"),new("A3","a1") },
                    new List<Element_to_target<string, string>> {new("B1","b1"),new("B2","b2") }
                },
                new(){
                    new List<Element_to_target<string, string>> {new("A1","a2"),new("A2","a3"),new("A3","a1") },
                    new List<Element_to_target<string, string>> {new("B1","b1"),new("B2","b2") }
                },
                new(){
                    new List<Element_to_target<string, string>> {new("A1","a4"),new("A2","a3"),new("A3","a1") },
                    new List<Element_to_target<string, string>> {new("B1","b1"),new("B2","b2") }
                },
                new(){
                    new List<Element_to_target<string, string>> {new("A1","a3"),new("A2","a1"),new("A3","a2") },
                    new List<Element_to_target<string, string>> {new("B1","b1"),new("B2","b2") }
                },
                new(){
                    new List<Element_to_target<string, string>> {new("A1","a4"),new("A2","a1"),new("A3","a2") },
                    new List<Element_to_target<string, string>> {new("B1","b1"),new("B2","b2") }
                },
                new(){
                    new List<Element_to_target<string, string>> {new("A1","a1"),new("A2","a3"),new("A3","a2") },
                    new List<Element_to_target<string, string>> {new("B1","b1"),new("B2","b2") }
                },
                new(){
                    new List<Element_to_target<string, string>> {new("A1","a4"),new("A2","a3"),new("A3","a2") },
                    new List<Element_to_target<string, string>> {new("B1","b1"),new("B2","b2") }
                },
            };
        
        CollectionAssert.AreEquivalent(
            result_combinations,
            generator
        );
        
        
    }
}



}