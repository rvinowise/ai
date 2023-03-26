using NUnit.Framework;
using rvinowise.ai.mapping_stencils;
using rvinowise.contracts;

namespace rvinowise.ai.test.generator_of_order_sequences {

[TestFixture]
public partial class filling_generator_or_orders_with_orders {
    
    
    
    [Test]
    public void all_combinations_are_provided_in_a_loop() {

        var generator = new Generator_of_orders<IEnumerable<Element_to_target<string,string>>>(
            new List<Generator_of_mappings<string, string>>{
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