
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
     IFigure create_next_figure(string prefix  = "");
     IFigure create_base_signal(string id = "");

     IFigure provide_sequence_for_pair(
          IFigure beginning_figure,
          IFigure ending_figure
     );

     IFigure find_figure_with_id(string id);
     void remove_figure(IFigure figure);
}
}