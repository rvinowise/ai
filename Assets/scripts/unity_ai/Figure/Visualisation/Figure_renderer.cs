using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Threading.Tasks;
using abstract_ai;
using rvinowise.ai.patterns;
using rvinowise.rvi.contracts;
using UnityEngine;

namespace rvinowise.unity.ai.figure {

public class Figure_renderer: MonoBehaviour {
    
    
    public Figure_storage figure_storage;
    private Figure figure;
    private String dot_file_template = 
$@"digraph {1} {{
layout=""dot""
rankdir=LR;

{2}

}}";
    
    public String get_dot_format(IFigure figure) {
        String node_connections
        = get_nodes_connections(figure);
        return String.Format(
            dot_file_template,
            "Figure",
             node_connections
        );
    }
    private String get_nodes_connections(IFigure figure) {
        return "a->b";
    }

    public void save_picture(IFigure figure) {
        System.IO.File.WriteAllText(
            $"Figure_{figure.id}.dot", 
            get_dot_format(figure)
        );
    }

    
    public void on_save_into_file() {
        foreach(
            IFigure figure in figure_storage.get_selected_figures()
        ) {
            save_picture(figure);
        }
    }
}
}