
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;

namespace rvinowise.ai.unity {

/* removes exessive connections, or adds them back */
public class Figure_pruner: MonoBehaviour {
    private Figure figure; 

    // it's ok to have ALL links stored (even through the nodes)
    // img: pruned vs unpruned connections in Figures

    public void on_prune() {
        foreach(var figure in Selector.instance.figures) {
            prune_exessive_links(figure);
        }
    }
    public void prune_exessive_links(IFigure figure) {
        foreach(IFigure_representation representation in figure.get_representations()) {
            prune_exessive_links(representation);
        }
    }
    public void prune_exessive_links(
       IFigure_representation representation
    ) {
        foreach(ISubfigure base_child in representation.get_subfigures()) {
            var reachebles_from_base = base_child.next;
            IReadOnlyList<ISubfigure> reacheble_from_next = 
                find_subfigures_reacheble_from_each_other(reachebles_from_base);
            remove_exsessive_connections(base_child, reacheble_from_next);
        }
    }

    private IReadOnlyList<ISubfigure> find_subfigures_reacheble_from_each_other(
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

    private void remove_exsessive_connections(
        ISubfigure start_subfigure, 
        IReadOnlyList<ISubfigure> excessive_next_subfigures
    ) {
        foreach(ISubfigure excessive_next in excessive_next_subfigures ) {
            start_subfigure.disconnect_from_next(excessive_next);
        }
    }
}
}