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

    private SortedDictionary<Element, Target> combination = new SortedDictionary<Element, Target>();
    private readonly SortedDictionary<Element, List<Target>> elements_to_targets;
    private readonly ISet<int> unassigned_elements = new SortedSet<int>();

    private int targets_number;
    private int elements_number {
        get => elements_to_targets.Count;
    }

    public Generator_of_individualised_mappings_enumerator(
        SortedDictionary<Element, List<Target>> elements_to_targets
    ) {
        this.elements_to_targets = elements_to_targets;
        Reset();
    }

    private bool first_combination() {

    }

    private bool mapping_has_shared_targets() {
        return combination.Values.Distinct().Count() < combination.Count()
    }

    private bool next_combination() {
        int i_element = 0;
        while (!has_next_free_targets(i_element)) 
        {
            if (its_last_element(i_element)) {
                return false;
            }
            prepare_element_for_resetting(i_element++);
        }
            
        move_one_step_forward(i_element);

        if (some_elements_are_resetting()) {
            settle_elements_at_the_beginning();
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

    private void set_to_first() {
        var targets = Enumerable.Range(
            0, elements_number
        ).Reverse().ToArray();
        occupy_targets_with_all_elements(targets);
    }


    private void occupy_targets_with_all_elements(int[] targets) {
        for (int element=0; element<elements_number; element++) {
            map_element_onto_target(element, targets[element]);
        }
    }
    
    
    
    private bool has_next_free_targets(int element) {
        int current_target_occupied_by_element = combination[element];
        if (current_target_occupied_by_element == -1) {
            return true; // only one target is needed - nothing else is occupied
        }
        int next_free_target = get_next_free_target(
            current_target_occupied_by_element
        );
        return next_free_target >= 0;
    }

    private int get_next_free_target(int previous_target) {
        bool[] free_targets = get_free_targets();
        for (int i=previous_target+1; i< free_targets.Length; i++) {
            if (free_targets[i]) {
                return i;
            }
        }
        return -1;
    }

    private bool[] get_free_targets() {
        bool[] free_targets = Enumerable.Repeat(true, targets_number).ToArray();
        for (int element = 0; element < elements_number; element++) {
            if (!unassigned_elements.Contains(element)) {
                if (combination[element] >= 0) { // this condition is needed only if one target is needed
                    free_targets[combination[element]] = false;
                }
            }
        }
        return free_targets;
    }

    private bool its_last_element(int element) {
        return element == elements_number-1;
    }

    private void prepare_element_for_resetting(int element) {
        unassigned_elements.Add(element);
    }

    private void move_one_step_forward(int element) {
        int current_target_occupied_by_order = combination[element];
        combination[element] = get_next_free_target(current_target_occupied_by_order);
    }
    

    private bool some_elements_are_resetting() {
        return unassigned_elements.Any();
    }

    private void settle_elements_at_the_beginning() {
        foreach (int reset_element in unassigned_elements.Reverse()) {
            map_element_onto_target(
                reset_element, 
                get_next_free_target(-1)
            );
            unassigned_elements.Remove(reset_element);
        }
    }

    private void map_element_onto_target(int element, int target) {
        combination[element] = target;
    }

    
}

}