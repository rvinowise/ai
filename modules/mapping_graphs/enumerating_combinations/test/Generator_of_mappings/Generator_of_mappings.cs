using System;
using NUnit.Framework;
using rvinowise.ai.generating_combinations;

namespace rvinowise.ai.test.Generator_of_mappings;

[TestFixture]
public class regular_loop_over_combinations_can_be_done {
    
    [Test]
    public void all_combinations_are_provided_in_a_loop() {
        Generator_of_mappings<string, string> generator = 
            new Generator_of_mappings<string, string>(
                new List<Element_to_targets<string, string>> {
                    new("A1", new List<string>{"a1","a2","a3","a4"}),
                    new("A2", new List<string>{"a1","a2","a3"}),
                    new("A3", new List<string>{"a1","a2"}),
                }
            );

        List<List<Element_to_target<string, string>>> result_combinations = [
            [new Element_to_target<string, string>("A1", "a3"), new Element_to_target<string, string>("A2", "a2"), new Element_to_target<string, string>("A3", "a1")],
            [new Element_to_target<string, string>("A1", "a4"), new Element_to_target<string, string>("A2", "a2"), new Element_to_target<string, string>("A3", "a1")],
            [new Element_to_target<string, string>("A1", "a2"), new Element_to_target<string, string>("A2", "a3"), new Element_to_target<string, string>("A3", "a1")],
            [new Element_to_target<string, string>("A1", "a4"), new Element_to_target<string, string>("A2", "a3"), new Element_to_target<string, string>("A3", "a1")],
            [new Element_to_target<string, string>("A1", "a3"), new Element_to_target<string, string>("A2", "a1"), new Element_to_target<string, string>("A3", "a2")],
            [new Element_to_target<string, string>("A1", "a4"), new Element_to_target<string, string>("A2", "a1"), new Element_to_target<string, string>("A3", "a2")],
            [new Element_to_target<string, string>("A1", "a1"), new Element_to_target<string, string>("A2", "a3"), new Element_to_target<string, string>("A3", "a2")],
            [new Element_to_target<string, string>("A1", "a4"), new Element_to_target<string, string>("A2", "a3"), new Element_to_target<string, string>("A3", "a2")]
        ];

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
        
        Generator_of_mappings<string, string> generator = 
            new Generator_of_mappings<string, string>(
                new List<Element_to_targets<string,string>> {
                    new("A1", new List<string>{"a1","a2","a3","a4"}),
                    new("A2", new List<string>{"a1"}),
                    new("A3", new List<string>{"a1"})
                }
            );
        
        Assert.IsEmpty(generator);
    }
}