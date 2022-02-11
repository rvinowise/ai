using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.rvi;
using UnityEngine;


namespace rvinowise.ai.unity.mapping_stencils {

public class Combination_for_figure {

    public int max_index;

    public int get_needed_amount() {
        return indexes.Length;
    }
    public int[] indexes;
    
    public Combination_for_figure(
        int max_index,
        int needed_amount
    ) {
        this.max_index = max_index;
        indexes = new int[needed_amount];
    }

    public void set_to_first() {
        indexes = Enumerable.Range(0, get_needed_amount()).ToArray();
    }

    public Combination_for_figure get_next() {
        bool increment_next_order = true;
        int i_order = 0;
        while (increment_next_order) {
            indexes[i_order]++;
            if (indexes[i_order] == max_index) {
                indexes[i_order] = 0;
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
        foreach (int index in indexes) {
            if (used_indexes.Contains(index)) {
                return true;
            }
            used_indexes.Add(index);
        }
        return false;
    }

    public bool is_last() {
        for (int i_subfigure = 0; i_subfigure < get_needed_amount(); i_subfigure++) {
            if (indexes[i_subfigure] != max_index - i_subfigure) {
                return false;
            }
        }
        return true;
    }
}

public class Subnodes_combination {
    public Dictionary<IFigure, Combination_for_figure> mapping = 
        new Dictionary<IFigure, Combination_for_figure>();

    public Subnodes_combination() {
            
    }
        
    public void add_mapping_of_figure(
    IFigure figure,
    Combination_for_figure combination
    ) {
        mapping[figure] = combination;
    }

    public Subnodes_combination set_to_first() {
        
    }
    
    public Subnodes_combination get_next() {
        
    }
    
    public bool is_last() {
        
    }
}


public class Mapped_nodes_combinator: MonoBehaviour {
    
    
   

    private IList<Stencil_mapping> recombine_subnodes_as_mappings(
        IList<IList<ISubfigure>> subnode_occurances
    ) {
        IList<Stencil_mapping> potential_mappings = new List<Stencil_mapping>();
        
        
    }

    class Subnode_occurances {
        public IList<IList<ISubfigure>> subnodes;
    }


    private void get_next_index_combination(Index_combination previous_combination) {
        for (int i=0;i< previous_combination.indexes.Count; i++) {
            previous_combination.indexes[i]++;
        }
    }

    
    
    Combination_for_figure get_first_combination_for_figure(
        Combination_for_figure combination
    ) {
        combination.set_to_first();
        return combination;
    }
    Combination_for_figure get_next_combination_for_figure(
        Combination_for_figure combination
    ) {
        combination
    }

    

    public Subnodes_combination get_first_combination(
        Subnodes_combination s_ubnodes_combination
    ) {
        
    }
    
    public Subnodes_combination get_next_combination(
        Subnodes_combination s_ubnodes_combination
    ) {
        
    }
}


}