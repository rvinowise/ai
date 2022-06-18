using System.Collections.Generic;
using rvinowise.ai.general;
using UnityEngine;

namespace rvinowise.ai.unity {
public abstract class Visual_input_receiver:
MonoBehaviour
,IInput_receiver
{
    
    public abstract void input_selected_signals();
    public abstract void input_signals(IEnumerable<IFigure> signals, int mood_change = 0);
    public abstract void start_new_line(); 
}
}