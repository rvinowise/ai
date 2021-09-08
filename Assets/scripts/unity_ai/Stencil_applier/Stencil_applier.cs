using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using rvinowise.unity.ai.action;
using UnityEngine;
using rvinowise.unity.extensions;
using Action = rvinowise.unity.ai.action.Action;
using rvinowise.rvi.contracts;
using rvinowise.ai.patterns;
using System.Numerics;
using abstract_ai;

namespace rvinowise.unity.ai {


public class Stencil_applier: MonoBehaviour {
    
    
    public IList<IFigure> apply_stencil(Stencil stencil, IFigure target) {
        IList<Stencil_projection> projections 
        = project_stencil_on_target(stencil, target);

        foreach(Stencil_projection projection in projections) {
            IList<Figure> out_figures
            = extract_figures_out_of_projected_stencils(projection)
        }
        
        foreach(Subfigure subfigure in stencil.first_subfigures) {
            IList<Subfigure> matching_figures
            = target.get_subfigures_of_type(
                subfigure.figure
            );
            matching_figures.
        }
    }

    private IList<Stencil_projection> project_stencil_on_target(
        Stencil stencil, IFigure target
    ) {
        IList<Stencil_projection> = new List<Stencil_projection>();

    }
}


}