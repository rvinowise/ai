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
    private readonly ISet<int> unassigned_orders = new SortedSet<int>();

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
        int i_order = 0;
        while (!has_next_free_targets(i_order)) 
        {
            if (its_last_order(i_order)) {
                return false;
            }
            prepare_order_for_resetting(i_order++);
        }
            
        move_one_step_forward(i_order);

        if (some_orders_are_resetting()) {
            settle_orders_at_the_beginning();
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
        var occurences = Enumerable.Range(
            0, elements_number
        ).Reverse().ToArray();
        occupy_occurences_with_all_orders(occurences);
    }


    private void occupy_occurences_with_all_orders(int[] occurences) {
        //contracts.Contract.Requires(occurences.Length == get_needed_amount());
        for (int order=0; order<elements_number; order++) {
            occupy_occurence_with_order(order, occurences[order]);
        }
    }
    
    
    
    private bool has_next_free_targets(int order) {
        int current_target_occupied_by_order = combination[order];
        if (current_target_occupied_by_order == -1) {
            return true; // only one occurence is needed - nothing else is occupied
        }
        int next_free_occurence = get_next_free_occurence(
            current_target_occupied_by_order
        );
        return next_free_occurence >= 0;
    }

    private int get_next_free_occurence(int previous_occurence) {
        bool[] free_occurences = get_free_occurences();
        for (int i=previous_occurence+1; i< free_occurences.Length; i++) {
            if (free_occurences[i]) {
                return i;
            }
        }
        return -1;
    }

    private bool[] get_free_occurences() {
        bool[] free_occurences = Enumerable.Repeat(true, targets_number).ToArray();
        for (int order = 0; order < elements_number; order++) {
            if (!unassigned_orders.Contains(order)) {
                if (combination[order] >= 0) { // this condition is needed only if one occurence is needed
                    free_occurences[combination[order]] = false;
                }
            }
        }
        return free_occurences;
    }

    private bool its_last_order(int order) {
        return order == elements_number-1;
    }

    private void prepare_order_for_resetting(int order) {
        unassigned_orders.Add(order);
    }

    private void move_one_step_forward(int order) {
        int current_occurence_occupied_by_order = combination[order];
        combination[order] = get_next_free_occurence(current_occurence_occupied_by_order);
    }
    

    private bool some_orders_are_resetting() {
        return unassigned_orders.Any();
    }

    private void settle_orders_at_the_beginning() {
        foreach (int reset_order in unassigned_orders.Reverse()) {
            occupy_occurence_with_order(
                reset_order, 
                get_next_free_occurence(-1)
            );
            unassigned_orders.Remove(reset_order);
        }
    }

    private void occupy_occurence_with_order(int order, int occurence) {
        combination[order] = occurence;
    }

    
}

}