using System.Collections.Generic;
using rvinowise.ai.general;
using UnityEngine;

namespace rvinowise.ai.unity {

public abstract class Input: MonoBehaviour {

    public Visual_input_receiver receiver;
    public Pattern_storage pattern_storage;

    

    virtual protected void Awake() {
        pattern_storage = receiver.pattern_storage;
    }
    
 
}
}