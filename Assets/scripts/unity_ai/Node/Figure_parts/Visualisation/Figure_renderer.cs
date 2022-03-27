using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using rvinowise.ai.general;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.unity.ui.input;
using UnityEngine;

namespace rvinowise.ai.unity {

public class Figure_renderer: MonoBehaviour {
    
    
    public Figure_showcase figure_showcase;
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

    public String get_dot_format(Figure_representation representation) {
        String node_connections
        = get_nodes_connections(representation);
        return String.Format(
            dot_file_template,
            "Figure",
             node_connections
        );
    }
    private String get_nodes_connections(Figure_representation representation) {
        StringBuilder result = new StringBuilder();
        foreach (Subfigure subfigure in representation.subfigures) {
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

    
    public void save_picture(Figure_representation representation) {
        System.IO.File.WriteAllText(
            $"{out_path}Figure_{representation.id}.dot", 
            get_dot_format(representation)
        );
    }

    
    public void on_save_into_file() {
        foreach(
            IFigure figure in Selector.instance.figures
        ) {
            Figure_representation representation = ((Figure) figure).representations.First() as Figure_representation;
            save_picture(representation);
        }
    }
}
}