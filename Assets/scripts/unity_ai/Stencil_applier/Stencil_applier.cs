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
    //     IReadOnlyList<IAction_group> action_groups,
    //     Stencil stencil
    // ) {
    //     Stencil applied_stencil = this;
    //     IFigure stencil_subfigure = stencil.;
    //     foreach(
    //         IAction_group action_group in action_groups 
    //     ) {
    //         if (
    //             action_group.has_action<Appearance_start>(
    //                 stencil_subfigure
    //             )
    //         ) {

    //         }
    //     }
    // }

    // public static IList<IFigure> apply_stencil(
    //     Stencil stencil,
    //     IReadOnlyList<IAction_group> action_groups
        
    // ) {
    //     // stencils can be only applied to the Figures. 
    //     // conversion img: conversion of Action_istory into Figure
    //     IFigure figure = create_figure_from_action_history(action_groups);
    //     return apply_stencil(stencil, figure);
    // }

    

    // public IList<IFigure> apply_stencil(
    //     Stencil stencil, 
    //     IFigure target
    // ) {
    //     IList<Stencil_projection> projections 
    //     = project_stencil_on_target(stencil, target);

    //     foreach(Stencil_projection projection in projections) {
    //         IList<Figure> out_figures
    //         = extract_figures_out_of_projected_stencils(projection);
    //     }
        
    
    // }

    // private IList<Stencil_projection> project_stencil_on_target(
    //     Stencil stencil, IFigure target
    // ) {
    //     IList<Stencil_projection> result = new List<Stencil_projection>();

    //     return result;
    // }

    // IList<Figure> extract_figures_out_of_projected_stencils(
    //     Stencil_projection projection
    // ) {

    // }
}


}