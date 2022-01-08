using System;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using rvinowise.ai.general;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using UnityEngine;

namespace rvinowise.ai.unity {

public class Figure_renderer: MonoBehaviour {
    
    
    public Figure_storage figure_storage;
    private Figure figure;
    private String dot_file_template = 
@"digraph {0} {{
layout=""dot""
rankdir=LR;

{1}

}}";
    private String out_path;
    void Awake() {
        out_path = String.Format(
            "{0}/output/",
            Application.dataPath
        );
        DirectoryInfo out_path_info = Directory.CreateDirectory(out_path);
    }

    public String get_dot_format(Figure figure) {
        String node_connections
        = get_nodes_connections(figure);
        return String.Format(
            dot_file_template,
            "Figure",
             node_connections
        );
    }
    private String get_nodes_connections(Figure figure) {
        StringBuilder result = new StringBuilder();
        foreach (Subfigure subfigure in figure.subfigures) {
            write_next_nodes_for(subfigure, result);
        }
        return result.ToString();
    }

    private void write_next_nodes_for(Subfigure subfigure, StringBuilder result) {
        foreach (Subfigure next_sunfigure in subfigure.next) {
            result.AppendFormat(
                "\"{0}\" -> \"{1}\"\n",
                subfigure.get_name(),
                next_sunfigure.get_name()
            );
        }
    }

    
    public void save_picture(Figure figure) {
        System.IO.File.WriteAllText(
            $"{out_path}Figure_{figure.id}.dot", 
            get_dot_format(figure)
        );
    }

    
    public void on_save_into_file() {
        foreach(
            IFigure figure in figure_storage.get_selected_figures()
        ) {
            save_picture(figure as Figure);
        }
    }
}
}