
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;

namespace rvinowise.ai.general {

public interface IFigure_provider {

     IReadOnlyList<IFigure> get_known_figures();
     public IFigure provide_figure(string id);

     IFigure provide_sequence_for_pair(
          IFigure beginning_figure,
          IFigure ending_figure
     );

     IFigure find_figure_with_id(string id);
     public string get_next_id_for_prefix(string prefix);
    
     void remove_figure(IFigure figure);
}
}