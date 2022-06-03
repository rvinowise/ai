using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.rvi;

namespace rvinowise.ai.simple.mapping_stencils {


/*
    iterates in the numerical order, with each Order being another iterator
    (with Reset and MoveNext methods) 
    */

public class Generator_of_order_sequences<T>:
    IEnumerable<T[]>
{
    public List<IEnumerable<T>> orders = 
        new List<IEnumerable<T>>();

    private readonly bool is_valid = true;
    
    // public Generator_of_order_sequences(
    //     IList<Mapping_enumerator_requirement> figure_requirements
    // ) {
    //     for (int i_figure = 0; i_figure < figure_requirements.Count; i_figure++) {
    //         orders.Add(
    //             new Generator_of_mappings(
    //                 figure_requirements[i_figure].amount_in_source,
    //                 figure_requirements[i_figure].amount_in_target
    //             )
    //         );
    //     }
    // }

    public Generator_of_order_sequences() {

    }

    public void add_order(IEnumerable<T> in_order) {
        orders.Add(in_order);
    }

    public T[] get_combination_as_indexes(
        IEnumerable<IEnumerator<T>> mapping_enumerators
    ) {
        T[] result_orders = new T[mapping_enumerators.Count()];
        
        int i_figure=0;
        foreach(IEnumerator<T> figure_enumerator in mapping_enumerators) {
            result_orders[i_figure++] = figure_enumerator.Current;
        }

        return result_orders;
    }
    
    #region IEnumerable

    public IEnumerator<T[]> GetEnumerator() {
        if (!is_valid) {
            yield break;
        }
        IList<IEnumerator<T>> order_enumerators = orders.Select(
            order => order.GetEnumerator()
        ).ToList();
        foreach(var enumerator in order_enumerators) {
            enumerator.SetToFirst();
        }
        while (set_next_iteration(order_enumerators)) {
            yield return get_combination_as_indexes(order_enumerators);
        }
    }

    

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }

    #endregion IEnumerable

    private bool set_next_iteration(IEnumerable<IEnumerator> enumerators) {
        foreach (IEnumerator enumerator in enumerators) {
            if (enumerator.MoveNext()) {
                return true;
            } else {
                enumerator.SetToFirst();
            }
        }
        return false;
    }

    
}





}