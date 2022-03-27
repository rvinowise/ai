using System.Collections.Generic;
using rvinowise.ai.general;
using UnityEngine;

namespace rvinowise.ai.unity {
public abstract class Visual_input_receiver:
MonoBehaviour
,IInput_receiver
{
    
    public Figure_showcase figure_showcase;
    public abstract void input_selected_signals();
    public abstract void start_new_line(); 
}
}