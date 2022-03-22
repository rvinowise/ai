
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
  
     IFigure create_new_figure(string prefix = "");

     IFigure provide_sequence_for_pair(
          IFigure beginning_figure,
          IFigure ending_figure
     );

     void remove_figure(IFigure figure);
}
}