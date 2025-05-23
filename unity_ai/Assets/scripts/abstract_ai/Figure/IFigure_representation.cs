﻿using System.Collections.Generic;


namespace rvinowise.ai.general {

public interface IFigure_representation {

    string id { get; }
    
    IReadOnlyList<ISubfigure> get_subfigures();
    IReadOnlyList<ISubfigure> get_first_subfigures();
    void add_first_subfigures(ISubfigure subfigure);
    ISubfigure create_subfigure(IFigure child_figure);
}

}