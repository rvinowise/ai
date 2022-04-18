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



public class Subnodes_combinator:
    IEnumerable, 
    IEnumerator
{
    private readonly List<Combinator_for_figure> combinations_for_figures = 
        new List<Combinator_for_figure>();

    public bool is_valid = true;
    
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
            if (!combinations_for_figures.Last().is_valid) {
                is_valid = false;
                break;
            }
        }

        Reset();
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
        if (!is_valid) {
            return false;
        }
        bool last_order_overflows = true;
        foreach (Combinator_for_figure combination in combinations_for_figures) {
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
        combinations_for_figures.First().Reset();
        for(int i_combination = 1; i_combination < combinations_for_figures.Count; i_combination++) {
            combinations_for_figures[i_combination].set_to_first();
        }
    }

    private bool set_to_first() {
        foreach (
            var combination_for_figure 
            in combinations_for_figures
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
            in combinations_for_figures
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


public readonly struct Figure_combinator_figure_requirement {
    public readonly int needed_amount;
    public readonly int max_occurances;

    public Figure_combinator_figure_requirement(
        int needed_amount, int max_occurances
    ) {
        this.needed_amount = needed_amount;
        this.max_occurances = max_occurances;
    }
        
}


}