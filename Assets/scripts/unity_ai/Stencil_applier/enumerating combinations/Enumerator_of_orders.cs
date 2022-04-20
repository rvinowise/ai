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


/*
 iterates in the numerical order, with each Order being another iterator
 (with Reset and MoveNext methods) 
 */
public class Enumerator_of_orders:
    IEnumerable, 
    IEnumerator
{
    private readonly List<IEnumerator> orders = 
        new List<IEnumerator>();

    private readonly bool is_valid = true;
    
    public Enumerator_of_orders(
        IList<Mapping_enumerator_requirement> figure_requirements
    ) {
        for (int i_figure = 0; i_figure < figure_requirements.Count; i_figure++) {
            orders.Add(
                new Mapping_enumerator(
                    figure_requirements[i_figure].amount_in_source,
                    figure_requirements[i_figure].amount_in_target
                )
            );
            if (orders.Last().is_valid) continue;
            is_valid = false;
            break;
        }

        Reset();
    }

    public int[][] get_combination_as_indexes() {
        int[][] result_figures = new int[orders.Count][];
        for (int i_figure = 0; i_figure < orders.Count; i_figure++) {
            Mapping_enumerator combinator_for_figure = orders[i_figure];
            result_figures[i_figure] = combinator_for_figure.combination;
        }

        return result_figures;
    }
    
    #region IEnumerable IEnumerator

    public IEnumerator GetEnumerator() {
        return this;
    }

    public bool MoveNext() {
        if (!is_valid) {
            return false;
        }
        bool last_order_overflows = true;
        foreach (Mapping_enumerator combination in orders) {
            if (combination.MoveNext()) {
                last_order_overflows = false;
                break;
            }
            combination.set_to_first();
        }
        if (last_order_overflows) {
            return false;
        }
        return true;
    }

    public void Reset() {
        orders.First().Reset();
        for(int i_combination = 1; i_combination < orders.Count; i_combination++) {
            orders[i_combination].set_to_first();
        }
    }

    private bool set_to_first() {
        foreach (
            var combination_for_figure 
            in orders
        ) {
            if (!combination_for_figure.set_to_first()) {
                return false;
            }
        }
        return true;
    }

    private bool set_to_last() {
        foreach (
            var combination_for_figure 
            in orders
        ) {
            if (!combination_for_figure.set_to_last()) {
                return false;
            }
        }
        return true;
    }


    public object Current {
        get => get_combination_as_indexes();
    }
    
    #endregion
}





}