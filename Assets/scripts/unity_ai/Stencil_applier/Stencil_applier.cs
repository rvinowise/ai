using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using rvinowise.ai.unity;
using rvinowise.unity.extensions;
using Action = rvinowise.ai.unity.Action;
using rvinowise.rvi.contracts;
using rvinowise.ai.general;
using System.Numerics;

namespace rvinowise.ai.unity {


public class Stencil_applier: MonoBehaviour {
    
    
    // public static IList<IFigure> apply_stencil(
    //     Stencil stencil,
    //     IReadOnlyList<IAction_group> action_groups
        
    // ) {
    //     // stencils can be only applied to the Figures. 
    //     // conversion img: conversion of Action_istory into Figure
    //     IFigure figure = create_figure_from_action_history(action_groups);
    //     return apply_stencil(stencil, figure);
    // }

    

    public IList<IFigure> apply_stencil(
        IStencil stencil, 
        IFigure_representation target
    ) {
        IList<Stencil_mapping> mappings = 
            map_stencil_onto_target(stencil, target);

        foreach(Stencil_mapping mapping in mappings) {
            IReadOnlyList<IFigure> out_figures = 
                extract_figures_out_of_projected_stencils(mapping);
        }
        
    
    }

    private IList<Stencil_mapping> map_stencil_onto_target(
        IStencil stencil, IFigure_representation target
    ) {
        IList<Stencil_mapping> potential_mappings = map_first_nodes(stencil, target);

        for(int i_node = 1; i_node < stencil.get_subfigures().Count; i_node++) {
            ISubfigure subfigure = stencil.get_subfigures()[i_node];
            map_next_node(potential_mappings, subfigure, target);
        }
        
        return potential_mappings;
    }

    private IList<Stencil_mapping> map_first_nodes(
        IStencil stencil, IFigure_representation target
    ) {
        
        IList<IList<ISubfigure>> subnode_occurances = 
            get_all_subnodes_occurances(stencil, target);
        

        IList<Stencil_mapping> potential_mappings = 
            recombine_subnodes_as_mappings(subnode_occurances);

        return potential_mappings;
    }

    private IList<IList<ISubfigure>> get_all_subnodes_occurances(
        IStencil stencil, IFigure_representation target
    ) {
        IList<IList<ISubfigure>> subnode_occurances = new List<IList<ISubfigure>>();
        foreach (ISubfigure subfigure in stencil.get_first_subfigures()) {
            IList<ISubfigure> occurances = get_occurances_of_subnode_in_graph(
                subfigure.referenced_figure, target
            );
            subnode_occurances.Add(occurances);
        }
        return subnode_occurances;
    }

    private IList<Stencil_mapping> recombine_subnodes_as_mappings(
        IList<IList<ISubfigure>> subnode_occurances
    ) {
        IList<Stencil_mapping> potential_mappings = new List<Stencil_mapping>();
        
        
    }

    class Subnode_occurances {
        public IList<IList<ISubfigure>> subnodes;
    }

    struct Index_combination {
        public List<int> indexes;
    }

    private void get_next_index_combination(Index_combination previous_combination) {
        for (int i=0;i< previous_combination.indexes.Count; i++) {
            previous_combination.indexes[i]++;
        }
    }

    struct Combination_for_figure {

        public Combination_for_figure(
            int occurences_in_target,
            int needed_amount
        ) {
            this.occurences_in_target = occurences_in_target;
            combined_indexes = new int[needed_amount];
        }
        
        public int occurences_in_target;

        public int get_needed_amount() {
            return combined_indexes.Length;
        }
        public int[] combined_indexes;
    }
    
    Combination_for_figure get_first_combination_of(
        Combination_for_figure combination
    ) {
        
    }
    Combination_for_figure get_next_combination_of(
        Combination_for_figure combination
    ) {
        
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

    IReadOnlyList<IFigure> extract_figures_out_of_projected_stencils(
        Stencil_mapping mapping
    ) {
        List<Figure> extracted_figures = new List<Figure>();


        return extracted_figures.AsReadOnly();
    }
}


}