﻿using System;
 using System.IO;
using System.Linq;
 using System.Text;
 using rvinowise.ai.general;

namespace rvinowise.ai.simple {

public class Figure_painted_into_dot_file {


    private String dot_file_template = 
@"digraph {0} {{
layout=""dot""
rankdir=LR;

{1}

}}";
    
    public Figure_painted_into_dot_file(IFigure figure, string out_path) {
        // out_path = String.Format(
        //     "{0}/output/",
        //     Application.dataPath
        // );
        Directory.CreateDirectory(out_path);
        save_figure_into_file(figure, out_path);
    }

    private String get_dot_format(IFigure_representation representation) {
        String node_connections
        = get_nodes_connections(representation);
        return String.Format(
            dot_file_template,
            "Figure",
             node_connections
        );
    }
    private String get_nodes_connections(IFigure_representation representation) {
        StringBuilder result = new StringBuilder();
        foreach (ISubfigure subfigure in representation.get_subfigures()) {
            write_next_nodes_for(subfigure, result);
        }
        return result.ToString();
    }

    private void write_next_nodes_for(ISubfigure subfigure, StringBuilder result) {
        foreach (ISubfigure next_sunfigure in subfigure.next) {
            result.AppendFormat(
                "\"{0}\" -> \"{1}\"\n",
                subfigure.id,
                next_sunfigure.id
            );
        }
    }


    private void save_picture_of_figure_representation(
        Figure_representation representation,
        string out_path
    ) {
        File.WriteAllText(
            $"{out_path}Figure_{representation.id}.dot", 
            get_dot_format(representation)
        );
    }

    
    private void save_figure_into_file(IFigure figure, string out_path) {
        Figure_representation representation = 
            figure.get_representations().First() as Figure_representation;
        save_picture_of_figure_representation(representation, out_path);
    }
}
}