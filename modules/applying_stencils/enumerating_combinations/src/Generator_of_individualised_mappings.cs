using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace rvinowise.ai.mapping_stencils {

public struct Element_to_targets<Element, Target> {
    public Element element;
    public List<Target> targets;
}
public struct Element_to_target<Element, Target> {
    public Element element;
    public Target target;

    public Element_to_target(Element element, Target target) {
        this.element = element;
        this.target = target;
    }
}
/* same as Generator_of_mappings, but with different possible targets for every element */
public class Generator_of_individualised_mappings<Element, Target>
    :IEnumerable<List<Element_to_target<Element, Target>>>
    where Element: notnull 
{

    private readonly List<Element_to_targets<Element,Target>> elements_to_targets = 
        new List<Element_to_targets<Element, Target>>();

    public Generator_of_individualised_mappings(
        List<Element_to_targets<Element, Target>> elements_to_targets
    ) {
        this.elements_to_targets = elements_to_targets;
    }

    #region IEnumerable

    public IEnumerator<List<Element_to_target<Element, Target>>> GetEnumerator() {
        return new Generator_of_individualised_mappings_enumerator<Element, Target>(elements_to_targets);
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    #endregion IEnumerable
    
}

class Target_counter<Target> {
    public int current =0;
    public List<Target> targets;

    public Target_counter(List<Target> targets) {
        contracts.Contract.Requires(targets.Any(), 
            "every mapped element should have at least one possible target");
        this.targets = targets;
    }

    public bool MoveNext() {
        current++;
        if (current >= targets.Count) {
            return false;
        }
        return true;
    }
    public void SetToFirst() {
        current=0;
    }

    public Target current_target() {
        return targets[current];
    }

}

public class Generator_of_individualised_mappings_enumerator<Element, Target>
    :IEnumerator<List<Element_to_target<Element, Target>>>
    where Element: notnull
{

    private readonly List<Element_to_target<Element, Target>> combination = new();

    private readonly List<Tuple<Element, Target_counter<Target>>> elements_to_targets;
    public Generator_of_individualised_mappings_enumerator(
        List<Element_to_targets<Element, Target>> elements_to_targets
    ) {
        this.elements_to_targets = 
            new List<Tuple<Element, Target_counter<Target>>>();
        foreach(var element_to_targets in elements_to_targets) {
            this.elements_to_targets.Add(
                Tuple.Create(
                    element_to_targets.element,
                    new Target_counter<Target>(element_to_targets.targets)
                )
            );
        }
        Reset();
    }

    private void set_combination_to_current_targets() {
        foreach(var element_to_target in elements_to_targets) {
            var element = element_to_target.Item1;

            var current_target =
                element_to_target.Item2.current_target();

            combination.Add(new Element_to_target<Element, Target>(element, current_target));
        }
    }
    
  
    private bool first_combination() {
        bool next_combination_exists = true;
        while (
            !correct_combination()
            &&
            next_combination_exists
        ) {
            next_combination_exists = next_mapping();
        }

        return next_combination_exists;
    }

    private bool correct_combination() { 
        set_combination_to_current_targets();
        return combination.Values.Distinct().Count() == combination.Count;
    }

    private bool next_mapping() {
        using var all_elements = elements_to_targets.GetEnumerator();
        all_elements.MoveNext();

        while (!all_elements.Current.Value.MoveNext()) {
            all_elements.Current.Value.SetToFirst();
            if (!all_elements.MoveNext()) {
                return false;
            }
        }
            
        return true;
    }


    #region IEnumerator
    public bool MoveNext()
    {
        bool exist;
        if (!combination.Any()) {
            exist = first_combination();
        } else {
            exist = next_mapping();
        }

        while (
            !correct_combination()
            &&
            exist
        ) {
            exist = next_mapping();
        }
        return exist;
    }

    public void Reset()
    {
        combination.Clear();
    }

    SortedDictionary<Element, Target> IEnumerator<SortedDictionary<Element, Target>>.Current {
        get { return combination; }
    }

    object IEnumerator.Current {
        get { return combination; }
    }
    
    public void Dispose() {
    }
    
    #endregion IEnumerator

    

    
}

}