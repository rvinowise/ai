using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.rvi;
using UnityEngine;


namespace rvinowise.ai.unity.mapping_stencils {


public struct Combination_for_figure {
    public int[] indexes;
}
public class Combinator_for_figure {

    public int max_subnodes;

    public int get_needed_amount() {
        return combination.Length;
    }
    public int[] combination;
    
    public Combinator_for_figure(
        int max_index,
        int needed_amount
    ) {
        this.max_subnodes = max_index;
        combination = new int[needed_amount];
    }

    public void set_to_first() {
        combination = Enumerable.Range(0, get_needed_amount()).Reverse().ToArray();
    }
    public void set_to_last() {
        combination = Enumerable.Range(max_subnodes-get_needed_amount(), max_subnodes).ToArray();
    }

    public Combinator_for_figure get_next() {
        bool increment_next_order = true;
        int i_order = 0;
        while (increment_next_order) {
            combination[i_order]++;
            if (combination[i_order] == max_subnodes) {
                combination[i_order] = 0;
                i_order++;
            }
            else {
                increment_next_order = false;
            }
        }
        while (has_indexes_used_twice()) {
            return get_next();
        }

        return this;
    }

    private ISet<int> used_indexes = new HashSet<int>();
    private bool has_indexes_used_twice() {
        used_indexes.Clear();
        foreach (int index in combination) {
            if (used_indexes.Contains(index)) {
                return true;
            }
            used_indexes.Add(index);
        }
        return false;
    }

    public bool is_last() {
        for (int i_subfigure = 0; i_subfigure < get_needed_amount(); i_subfigure++) {
            if (combination[i_subfigure] != max_subnodes - i_subfigure) {
                return false;
            }
        }
        return true;
    }
}

public class Subnodes_combinator :
    IEnumerable
{
    public Subnodes_combinator(IStencil stencil, IFigure_representation target) {
        foreach (ISubfigure subfigure in stencil.get_first_subfigures()) {
            IList<ISubfigure> occurances = get_occurances_of_subnode_in_graph(
                subfigure.referenced_figure, target
            );
            subnode_occurances.Add(occurances);
        }
    }
    
    private IList<ISubfigure> get_occurances_of_subnode_in_graph(
    IFigure figure, IFigure_representation target
    ) {
        IList<ISubfigure> occurances =
            target.get_subfigures().Where(subnode => 
                subnode.referenced_figure == figure
            ).ToList();
        return occurances;

    }
    
}
public class Subnodes_combination:
    IEnumerable, 
    IEnumerator
{
    public Dictionary<IFigure, Combinator_for_figure> combinations_for_figures = 
        new Dictionary<IFigure, Combinator_for_figure>();

    private bool is_reset = true;
    
    public Subnodes_combination() {
            
    }
        
    public void add_mapping_of_figure(
        IFigure figure,
        Combinator_for_figure combinator
    ) {
        combinations_for_figures[figure] = combinator;
    }
    
    public bool is_last() {
        foreach (
            var combination_for_figure 
            in combinations_for_figures.Values
        ) {
            if (!combination_for_figure.is_last()) {
                return false;
            }
        }
        return true;
        
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
        foreach (Combinator_for_figure combination in combinations_for_figures.Values) {
            if (combination.is_last()) {
                combination.set_to_first();
            }
            else {
                combination.get_next();
                last_order_overflows = false;
                break;
            }
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
            in combinations_for_figures.Values
        ) {
            combination_for_figure.set_to_first();
        }
    }
    public void set_to_last() {
        foreach (
            var combination_for_figure 
            in combinations_for_figures.Values
        ) {
            combination_for_figure.set_to_last();
        }
    }


    public object Current {
        get => this;
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


/*public class Mapped_nodes_combinator: MonoBehaviour {
    
    
   

    private IList<Stencil_mapping> recombine_subnodes_as_mappings(
        IList<IList<ISubfigure>> subnode_occurances
    ) {
        IList<Stencil_mapping> potential_mappings = new List<Stencil_mapping>();
        
        
    }

    class Subnode_occurances {
        public IList<IList<ISubfigure>> subnodes;
    }

    
}*/


}