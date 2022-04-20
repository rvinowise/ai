using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.rvi;
using UnityEngine;
using UnityEngine.Assertions;


namespace rvinowise.ai.unity.mapping_stencils {


public class Mapping_enumerator :
    IEnumerator 
{
    private readonly int amount_in_source;

    private readonly ISet<int> unassigned_orders = new SortedSet<int>();

    private int get_needed_amount() {
        return combination.Length;
    }

    public readonly int[] combination;

    public bool is_valid {
        get {
            return has_enough_subfigures();
        }
    }

    public Mapping_enumerator(
        int amount_in_source,
        int amount_in_target
    ) {
        // Contract.Requires(
        //     max_subnodes >= needed_amount,
        //     "impossible to provide any combinations with so few figure occurences"
        // );
        this.amount_in_source = amount_in_source;
        combination = new int[amount_in_target];
        Reset();
    }

    public bool set_to_first() {

        if (!is_valid) {
            return false;
        }
        
        var occurences = Enumerable.Range(
            0, get_needed_amount()
        ).Reverse().ToArray();
        occupy_occurences_with_all_orders(occurences);

        return true;
    }

    private bool has_enough_subfigures() {
        return get_needed_amount() <= amount_in_source;
    }
    
    public bool set_to_last() {
        if (!is_valid) {
            return false;
        }
        
        var occurences = Enumerable.Range(
            amount_in_source-get_needed_amount(), 
            get_needed_amount()
        ).ToArray();
        occupy_occurences_with_all_orders(occurences);
        
        return true;
    }

    private void occupy_occurences_with_all_orders(int[] occurences) {
        Contract.Requires(occurences.Length == get_needed_amount());
        for (int order=0; order<get_needed_amount(); order++) {
            occupy_occurence_with_order(order, occurences[order]);
        }
    }
    

    public bool is_last() {
        for (int i_subfigure = 0; i_subfigure < get_needed_amount(); i_subfigure++) {
            if (combination[i_subfigure] != amount_in_source - i_subfigure) {
                return false;
            }
        }
        return true;
    }
    
    #region IEnumerator

    public bool MoveNext() {
        if (!is_valid) {
            return false;
        }
        
        int i_order = 0;
        while (!has_next_free_occurances(i_order)) {
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
    
    public void Reset() {
        set_to_first();
        combination[0]--;
    }

    public object Current {
        get {
            return combination;
        }
    }
    

    private bool has_next_free_occurances(int order) {
        int current_occurance_occupied_by_order = combination[order];
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
        bool[] free_occurances = Enumerable.Repeat(true, amount_in_source).ToArray();
        for (int order = 0; order < get_needed_amount(); order++) {
            if (!unassigned_orders.Contains(order)) {
                free_occurances[combination[order]] = false;
            }
        }
        return free_occurances;
    }

    private bool its_last_order(int order) {
        return order == get_needed_amount()-1;
    }

    private void prepare_order_for_resetting(int order) {
        int current_occurance_occupied_by_order = combination[order];
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
    

    

    #endregion IEnumerator
}


public readonly struct Mapping_enumerator_requirement {
    public readonly int amount_in_target;
    public readonly int amount_in_source;

    public Mapping_enumerator_requirement(
        int amount_in_target, int amount_in_source
    ) {
        this.amount_in_target = amount_in_target;
        this.amount_in_source = amount_in_source;
    }

    public bool is_valid() {
        return amount_in_source >= amount_in_target;
    }
}

}