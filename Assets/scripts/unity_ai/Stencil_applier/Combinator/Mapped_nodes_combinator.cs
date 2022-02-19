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



public class Combinator_for_figure: 
    IEnumerator
{

    public int max_subnodes;

    private ISet<int> unassigned_orders = new SortedSet<int>();

    public int get_needed_amount() {
        return combination.Length;
    }
    public int[] combination;
    
    public Combinator_for_figure(
        int max_subnodes,
        int needed_amount
    ) {
        this.max_subnodes = max_subnodes;
        combination = new int[needed_amount];
        Reset();
    }

    public void set_to_first() {
        var occurences = Enumerable.Range(
            0, get_needed_amount()
        ).Reverse().ToArray();
        occupy_occurences_with_all_orders(occurences);

    }
    public void set_to_last() {
        var occurences = Enumerable.Range(
            max_subnodes-get_needed_amount(), 
            get_needed_amount()
        ).ToArray();
        occupy_occurences_with_all_orders(occurences);
    }

    private void occupy_occurences_with_all_orders(int[] occurences) {
        Contract.Requires(occurences.Length == get_needed_amount());
        for (int order=0; order<get_needed_amount(); order++) {
            occupy_occurence_with_order(order, occurences[order]);
        }
    }
    

    public bool is_last() {
        for (int i_subfigure = 0; i_subfigure < get_needed_amount(); i_subfigure++) {
            if (combination[i_subfigure] != max_subnodes - i_subfigure) {
                return false;
            }
        }
        return true;
    }
    
    #region IEnumerator

    public bool MoveNext() {
        int i_order = 0;
        if (combination.SequenceEqual(new []{ 3,4,0})) {
            bool test = true;
        }
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
        bool[] free_occurances = Enumerable.Repeat(true, max_subnodes).ToArray();
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
        foreach (int reset_order in unassigned_orders) {
            occupy_occurence_with_order(
                reset_order, 
                get_next_free_occurance(-1)
            );
        }
        unassigned_orders.Clear();
    }

    private void occupy_occurence_with_order(int order, int occurance) {
        combination[order] = occurance;
    }
    

    public void Reset() {
        set_to_first();
        combination[0]--;
    }

    public object Current {
        get {
            return this;
        }
    }

    #endregion IEnumerator
}



public struct Figure_combinator_figure_requirement {
    public int needed_amount;
    public int max_occurances;

    public Figure_combinator_figure_requirement(
        int needed_amount, int max_occurances
    ) {
        this.needed_amount = needed_amount;
        this.max_occurances = max_occurances;
    }
        
}

public class Subnodes_combinator:
    IEnumerable, 
    IEnumerator
{
    public List<Combinator_for_figure> combinations_for_figures = 
        new List<Combinator_for_figure>();

    private bool is_reset = true;
    
    public Subnodes_combinator(
        Figure_combinator_figure_requirement[] figure_requirements
    ) {
        for (int i_figure = 0; i_figure < figure_requirements.Length; i_figure++) {
            combinations_for_figures.Add(
                new Combinator_for_figure(
                    figure_requirements[i_figure].max_occurances,
                    figure_requirements[i_figure].needed_amount
                )
            );
        }

        Reset();
    }
    
    public bool is_last() {
        foreach (
            var combination_for_figure 
            in combinations_for_figures
        ) {
            if (!combination_for_figure.is_last()) {
                return false;
            }
        }
        return true;
    }

    public int[][] get_combination_as_indexes() {
        int[][] result_figures = new int[combinations_for_figures.Count][];
        for (int i_figure = 0; i_figure < combinations_for_figures.Count; i_figure++) {
            Combinator_for_figure combinator_for_figure = combinations_for_figures[i_figure];
            result_figures[i_figure] = combinator_for_figure.combination;
        }

        return result_figures;
    }
    
    #region IEnumerable IEnumerator

    public IEnumerator GetEnumerator() {
        return this;
    }

    public bool MoveNext() {
        if (is_reset) {
            is_reset = false;
            set_to_first();
            return true;
        }
        bool last_order_overflows = true;
        foreach (Combinator_for_figure combination in combinations_for_figures) {
            if (combination.MoveNext()) {
                last_order_overflows = false;
                break;
            }
            combination.set_to_first();
        }
        if (last_order_overflows && !is_reset) {
            return false;
        }
        return true;
    }

    public void Reset() {
        is_reset = true;
        set_to_last();
    }
    
    public void set_to_first() {
        foreach (
            var combination_for_figure 
            in combinations_for_figures
        ) {
            combination_for_figure.set_to_first();
        }
    }
    public void set_to_last() {
        foreach (
            var combination_for_figure 
            in combinations_for_figures
        ) {
            combination_for_figure.set_to_last();
        }
    }


    public object Current {
        get => get_combination_as_indexes();
    }
    
    #endregion
}

public class Subnodes_combination_enum :
    IEnumerator
{
    public bool MoveNext() {
        throw new NotImplementedException();
    }

    public void Reset() {
        throw new NotImplementedException();
    }

    public object Current { get; }
}


}