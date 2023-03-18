using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace rvinowise.ai.mapping_stencils {

/* same as Generator_of_mappings, but with different possible targets for every element */
public class Generator_of_individualised_mappings<Element, Target>
    :IEnumerable<SortedDictionary<Element, Target>>
    where Element: notnull 
    {
    

    private readonly SortedDictionary<Element, List<Target>> elements_to_targets = 
        new SortedDictionary<Element, List<Target>>();

    public Generator_of_individualised_mappings(
        SortedDictionary<Element, List<Target>> elements_to_targets
    ) {
        this.elements_to_targets = elements_to_targets;
    }

    #region IEnumerable

    public IEnumerator<SortedDictionary<Element, Target>> GetEnumerator() {
        return new Generator_of_individualised_mappings_enumerator<Element, Target>(elements_to_targets);
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    #endregion IEnumerable
    
}


public class Generator_of_individualised_mappings_enumerator<Element, Target>
    :IEnumerator<SortedDictionary<Element, Target>>
    where Element: notnull
{

    private SortedDictionary<Element, Target> combination = 
        new SortedDictionary<Element, Target>();
    private readonly SortedDictionary<Element, List<Target>> elements_to_targets;
    private readonly ISet<int> unassigned_elements = new SortedSet<int>();

    private int targets_number;
    private int elements_number {
        get => elements_to_targets.Count;
    }
    private SortedDictionary<Element,List<Target>.Enumerator> elements_to_enumerators;
    public Generator_of_individualised_mappings_enumerator(
        SortedDictionary<Element, List<Target>> elements_to_targets
    ) {
        this.elements_to_targets = elements_to_targets;
        elements_to_enumerators = 
            new SortedDictionary<Element, List<Target>.Enumerator>();
        foreach(var pair in elements_to_targets) {
            elements_to_enumerators.Add(
                pair.Key,
                pair.Value.GetEnumerator()
            );
        }
        Reset();
    }

    private void set_combination_to_current_enumerators() {
        foreach(var element_to_enumerator in elements_to_enumerators) {
            var element = element_to_enumerator.Key;

            var current_target = 
                element_to_enumerator.Value.Current;

            combination.Add(element, current_target);
        }
    }
    
    private void set_all_enumerators_to_first() {
        foreach(var element_to_enumerator in elements_to_enumerators) {
            element_to_enumerator.Value.SetToFirst();
        }
    }
    private bool first_combination() {
        set_all_enumerators_to_first();
        bool next_combination_exists = true;
        while (
            mapping_has_shared_targets()
            &&
            next_combination_exists
            ) {
            next_combination_exists = next_combination();
        }

        return next_combination_exists;
    }

    private bool mapping_has_shared_targets() {
        return combination.Values.Distinct().Count() < combination.Count();
    }

    private bool next_combination() {
        var all_enumerators = elements_to_enumerators.GetEnumerator();
        while (!all_enumerators.Current.Value.MoveNext()) {
            all_enumerators.Current.Value.SetToFirst();
            if (!all_enumerators.MoveNext()) {
                return false;
            }
            all_enumerators.MoveNext();
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
            exist = next_combination();
        }

        while (
            mapping_has_shared_targets()
            &&
            exist
        ) {
            exist = next_combination();
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