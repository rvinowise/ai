using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


namespace rvinowise.ai.mapping_stencils {


public class Generator_of_mappings:
    IEnumerable<int[]> {
    
    public readonly int available_amount;
    public readonly int taken_amount;

    public Generator_of_mappings(
        int taken_amount,
        int available_amount
    ) {
        contracts.Contract.Requires<ArgumentException>(
            taken_amount <= available_amount,
            "impossible to provide any combinations with so few figure occurences"
        );
        this.taken_amount = taken_amount;
        this.available_amount = available_amount;

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

    private readonly Generator_of_mappings generator;
    private int available_amount => generator.available_amount;

    private int get_needed_amount() {
        return combination.Length;
    }
    

    

    public Generator_of_mappings_enumerator(Generator_of_mappings generator) {
        this.generator = generator;
        combination = new int[generator.taken_amount];
        Reset();
    }

    
    #region IEnumerator
    public bool MoveNext()
    {
        int i_order = 0;
        while (!has_next_free_occurances(i_order)) 
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
            0, get_needed_amount()
        ).Reverse().ToArray();
        occupy_occurences_with_all_orders(occurences);
    }


    private void occupy_occurences_with_all_orders(int[] occurences) {
        //contracts.Contract.Requires(occurences.Length == get_needed_amount());
        for (int order=0; order<get_needed_amount(); order++) {
            occupy_occurence_with_order(order, occurences[order]);
        }
    }
    
    
    
    private bool has_next_free_occurances(int order) {
        int current_occurance_occupied_by_order = combination[order];
        if (current_occurance_occupied_by_order == -1) {
            return true; // only one occurance is needed - nothing else is occupied
        }
        int next_free_occurance = get_next_free_occurance(
            current_occurance_occupied_by_order
        );
        return next_free_occurance >= 0;
    }

    private int get_next_free_occurance(int previous_occurance) {
        bool[] free_occurances = get_free_occurances();
        for (int i=previous_occurance+1; i< free_occurances.Length; i++) {
            if (free_occurances[i]) {
                return i;
            }
        }
        return -1;
    }

    private bool[] get_free_occurances() {
        bool[] free_occurances = Enumerable.Repeat(true, available_amount).ToArray();
        for (int order = 0; order < get_needed_amount(); order++) {
            if (!unassigned_orders.Contains(order)) {
                if (combination[order] >= 0) { // this condition is needed only if one occurance is needed
                    free_occurances[combination[order]] = false;
                }
            }
        }
        return free_occurances;
    }

    private bool its_last_order(int order) {
        return order == get_needed_amount()-1;
    }

    private void prepare_order_for_resetting(int order) {
        unassigned_orders.Add(order);
    }

    private void move_one_step_forward(int order) {
        int current_occurance_occupied_by_order = combination[order];
        combination[order] = get_next_free_occurance(current_occurance_occupied_by_order);
    }
    

    private bool some_orders_are_resetting() {
        return unassigned_orders.Any();
    }

    private void settle_orders_at_the_beginning() {
        foreach (int reset_order in unassigned_orders.Reverse()) {
            occupy_occurence_with_order(
                reset_order, 
                get_next_free_occurance(-1)
            );
            unassigned_orders.Remove(reset_order);
        }
    }

    private void occupy_occurence_with_order(int order, int occurance) {
        combination[order] = occurance;
    }

    
}

}