
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.unity.ui.input;

namespace rvinowise.ai.unity {

/* removes excessive connections, or adds them back */
public class Pruned_figure {
    private Figure figure; 

    // it's ok to have ALL links stored (even through the nodes)
    // img: pruned vs unpruned connections in Figures

    public Pruned_figure(IFigure figure) {
        prune_excessive_links(figure);
    }

    private void prune_excessive_links(IFigure figure) {
        foreach(IFigure_representation representation in figure.get_representations()) {
            prune_excessive_links(representation);
        }
    }

    private void prune_excessive_links(
       IFigure_representation representation
    ) {
        foreach(ISubfigure base_child in representation.get_subfigures()) {
            var reachables_from_base = base_child.next;
            IReadOnlyList<ISubfigure> reachable_from_next = 
                find_subfigures_reachable_from_each_other(reachables_from_base);
            remove_excsessive_connections(base_child, reachable_from_next);
        }
    }

    private IReadOnlyList<ISubfigure> find_subfigures_reachable_from_each_other(
        IReadOnlyList<ISubfigure> subfigures
    ) {
        List<ISubfigure> result = new List<ISubfigure>();
        foreach(ISubfigure start_subfigure in subfigures) {
            foreach(ISubfigure end_subfigure in subfigures) {
                if (start_subfigure == end_subfigure) {
                    continue;
                }
                if (start_subfigure.next.Contains(end_subfigure)) {
                    result.Add(end_subfigure);
                }
            }
        }
        return result.AsReadOnly();
    }

    private void remove_excsessive_connections(
        ISubfigure start_subfigure, 
        IEnumerable<ISubfigure> excessive_next_subfigures
    ) {
        foreach(ISubfigure excessive_next in excessive_next_subfigures ) {
            start_subfigure.disconnect_from_next(excessive_next);
        }
    }
}
}