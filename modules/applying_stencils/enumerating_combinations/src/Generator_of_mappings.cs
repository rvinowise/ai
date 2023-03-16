using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace rvinowise.ai.mapping_stencils {


public class Generator_of_mappings:
    IEnumerable<int[]> {
    
    public readonly int elements_number;
    public readonly int targets_number;

    public Generator_of_mappings(
        int elements_number,
        int targets_number
    ) {
        contracts.Contract.Requires<ArgumentException>(
            elements_number <= targets_number,
            "impossible to provide any combinations with so few figure occurences"
        );
        this.elements_number = elements_number;
        this.targets_number = targets_number;

    }

    #region IEnumerable

    public IEnumerator<int[]> GetEnumerator() {
        return new Generator_of_mappings_enumerator(this);
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    #endregion IEnumerable
    
}


public class Generator_of_mappings_enumerator : IEnumerator<int[]>
{

    private readonly int[] combination;
    private readonly ISet<int> unassigned_elements = new SortedSet<int>();

    private int targets_number;
    private int elements_number {
        get => combination.Length;
    }

    public Generator_of_mappings_enumerator(Generator_of_mappings generator) {
        combination = new int[generator.elements_number];
        targets_number = generator.targets_number;
        Reset();
    }

    
    #region IEnumerator
    public bool MoveNext()
    {
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

    public void Reset()
    {
        set_to_first();
        combination[0]--;
    }

    int[] IEnumerator<int[]>.Current {
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