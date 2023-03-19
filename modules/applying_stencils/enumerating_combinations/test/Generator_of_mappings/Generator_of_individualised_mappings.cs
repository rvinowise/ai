using System;
using NUnit.Framework;
using rvinowise.ai.mapping_stencils;

namespace rvinowise.ai.test.generator_of_individualised_mappings {

[TestFixture]
public partial class regular_loop_over_combinations_can_be_done {
    
    [Test]
    public void all_combinations_are_provided_in_a_loop() {
        Generator_of_individualised_mappings<string, string> generator = 
            new Generator_of_individualised_mappings<string, string>(
                new SortedDictionary<string, List<string>> {
                    {"A1", new List<string>{"a1","a2","a3","a4"}},
                    {"A2", new List<string>{"a1","a2","a3"}},
                    {"A3", new List<string>{"a1","a2"}}
                }
            );

        List<SortedDictionary<string, string>> result_combinations = 
            new List<SortedDictionary<string, string>> {
                new() {{"A1","a3"},{"A2","a2"},{"A3","a1"} },
                new() {{"A1","a4"},{"A2","a2"},{"A3","a1"} },
                new() {{"A1","a2"},{"A2","a3"},{"A3","a1"} },
                new() {{"A1","a4"},{"A2","a3"},{"A3","a1"} },
                new() {{"A1","a3"},{"A2","a1"},{"A3","a2"} },
                new() {{"A1","a4"},{"A2","a1"},{"A3","a2"} },
                new() {{"A1","a1"},{"A2","a3"},{"A3","a2"} },
                new() {{"A1","a4"},{"A2","a3"},{"A3","a2"} }
            };

        CollectionAssert.AreEquivalent(
            result_combinations,
            generator
        );
 
    }
}

[TestFixture]
public class not_enough_occurences_of_figure {
    
    [Test]
    public void zero_iterations_are_possible() {
        
         Generator_of_individualised_mappings<string, string> generator = 
            new Generator_of_individualised_mappings<string, string>(
                new SortedDictionary<string, List<string>> {
                    {"A1", new List<string>{"a1","a2","a3","a4"}},
                    {"A2", new List<string>{"a1"}},
                    {"A3", new List<string>{"a1"}}
                }
            );
        
        Assert.IsEmpty(generator);
    }
}



}