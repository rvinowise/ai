using System.Collections.Generic;
using rvinowise.ai.patterns;
using UnityEngine;

namespace rvinowise.unity.ai {
public abstract class Visual_input_receiver:
MonoBehaviour
,IInput_receiver
{
    
    public Pattern_storage pattern_storage;
    public abstract void input_selected_patterns();
    public abstract void start_new_line(); 
}
}