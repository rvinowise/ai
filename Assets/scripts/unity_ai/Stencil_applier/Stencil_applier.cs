using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Action = rvinowise.ai.unity.Action;
using rvinowise.rvi.contracts;
using rvinowise.ai.general;
using System.Numerics;
using rvinowise.ai.simple.mapping_stencils;

namespace rvinowise.ai.simple {


public class Stencil_applier {
    
    
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

        return null;
    }

    private IList<Stencil_mapping> map_stencil_onto_target(
        IStencil stencil, IFigure_representation target
    ) {
        IList<Stencil_mapping> potential_mappings = map_first_nodes(stencil, target);

        for(int i_node = 1; i_node < stencil.get_subfigures().Count; i_node++) {
            ISubfigure subfigure = stencil.get_subfigures()[i_node];
            //map_next_node(potential_mappings, subfigure, target);
        }
        
        return potential_mappings;
    }

    class First_subfigures_for_combination {
        
        // index = figure;
        // value = appearances of its subfigures in the beginning of the stencil

        public IList<IList<ISubfigure>> appearances_of_figures;
    }
    
    private IList<Stencil_mapping> map_first_nodes(
        IStencil stencil, IFigure_representation target
    ) {
        //get arrays of stencil's subnodes
        // index = figure;
        // value = appearances of its subfigures in the beginning of the stencil
        IList<IList<ISubfigure>> appearances_in_stencil = new List<IList<ISubfigure>>();
 
        // array of figures ->
        // array of their appearances in the stencil ->
        // target's subfigure onto which it's mapped 
        Generator_of_order_sequences<int[]> enumerator_of_orders = 
            new Generator_of_order_sequences<int[]>();
        foreach(var appearances_in_source in appearances_in_stencil) {
            IFigure mapped_figure = 
                appearances_in_source.First().referenced_figure;
            IReadOnlyList<ISubfigure> appearances_in_target = 
                get_appearances_of_figure_in_graph(mapped_figure, target);
            enumerator_of_orders.add_order(
                new Generator_of_mappings(
                    appearances_in_source.Count, appearances_in_target.Count
                ) 
            );
        }
        
        //transform iterations of combinator into potential mappings
        foreach (var combination in enumerator_of_orders) {
            var test = combination;
        }
        
        IList<IList<ISubfigure>> subnode_occurances = 
            get_all_subnodes_occurances(stencil, target);
        

        IList<Stencil_mapping> potential_mappings = 
            recombine_subnodes_as_mappings(subnode_occurances);

        return potential_mappings;
    }

    private IReadOnlyList<ISubfigure> get_appearances_of_figure_in_graph(IFigure figure, IFigure_representation graph) {
        List<ISubfigure> result = new List<ISubfigure>();
        
        foreach (ISubfigure subfigure in graph.get_subfigures()) {
            if (subfigure.referenced_figure == figure) {
                result.Add(subfigure);
            }
        }
        
        return result.AsReadOnly();

        
        //return graph.get_subfigures().(ISubfigure subfigure -> subfigure.refe)
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

        
        return potential_mappings;
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