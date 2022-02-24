using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.general;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Linq;
using rvinowise.ai.unity.simple;
using rvinowise.ai.unity.visuals;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.ui.input.mouse;

namespace rvinowise.ai.simple {

public class Figure_representation:
IFigure_representation
{
    #region IFigure_representation
    public string id{get;set;}
    public IReadOnlyList<ISubfigure> get_subfigures() => subfigures.AsReadOnly();
    public IReadOnlyList<ISubfigure> get_first_subfigures() => first_subfigures.AsReadOnly();
    public void add_first_subfigures(ISubfigure subfigure) => first_subfigures.Add(subfigure);
    #endregion IFigure_representation

    private readonly List<ISubfigure> first_subfigures = new List<ISubfigure>();

    private readonly List<ISubfigure> subfigures = new List<ISubfigure>();
    
    
    #region building
    public ISubfigure create_subfigure(IFigure child_figure) {
        Subfigure subfigure = Subfigure.
            create_for_figure(child_figure);
        attach_subfigure(subfigure);
        return subfigure;
    }
    public ISubfigure create_subfigure(Stencil_interface direction) {
        Subfigure subfigure = Subfigure.
            create_for_stencil_interface(direction);
        attach_subfigure(subfigure);
        return subfigure;
    }

    private void attach_subfigure(ISubfigure subfigure) {
        subfigure.parent = this;
        subfigures.Add(subfigure);
    }

   
    public void delete_subfigure(Subfigure subfigure) {
        delete_all_connections_touching(subfigure);
        subfigures.Remove(subfigure);
    }

    private void delete_all_connections_touching(Subfigure detached_subfigure) {
        foreach (Subfigure subfigure in subfigures) {
            if (detached_subfigure.next.Contains(subfigure)) {
                detached_subfigure.disconnect_from_next(subfigure);
            }
            if (subfigure.next.Contains(detached_subfigure)) {
                subfigure.disconnect_from_next(detached_subfigure);
            }
        }
    }
    
    #endregion building


}
}