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
        IList<Stencil_mapping> potential_mappings = map_first_node(stencil, target);

        for(int i_node = 1; i_node < stencil.get_subfigures().Count; i_node++) {
            ISubfigure subfigure = stencil.get_subfigures()[i_node];
            map_next_node(potential_mappings, subfigure, target);
        }
        
        return potential_mappings;
    }

    IReadOnlyList<IFigure> extract_figures_out_of_projected_stencils(
        Stencil_mapping mapping
    ) {
        List<Figure> extracted_figures = new List<Figure>();


        return extracted_figures.AsReadOnly();
    }
}


}