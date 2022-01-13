using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.general;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Linq;
using rvinowise.ai.unity.visuals;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.ui.input.mouse;

namespace rvinowise.ai.unity {

public class Sequential_figure: 
Figure 
{
    public IList<IFigure> sequence;

    public IReadOnlyList<IFigure> as_lowlevel_sequence() {
        if (sequence.Any()) {
            return sequence;
        }
        return new List<IFigure> {this};
    }

}
}