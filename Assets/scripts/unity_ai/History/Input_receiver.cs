using System.Collections.Generic;
using rvinowise.ai.general;
using UnityEngine;

namespace rvinowise.ai.unity {
public abstract class Visual_input_receiver:
MonoBehaviour
,IInput_receiver
{
    
    public Pattern_storage pattern_storage;
    public abstract void input_selected_figures();
    public abstract void start_new_line(); 
}
}