using System.Collections.Generic;
using UnityEngine;

namespace rvinowise.unity.ai.patterns {
public abstract class Input_receiver: MonoBehaviour {
    
    public Pattern_storage pattern_storage;
    
    public abstract void input_selected_patterns();
}
}