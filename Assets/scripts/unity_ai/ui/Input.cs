using System.Collections.Generic;
using rvinowise.ai.general;
using UnityEngine;

namespace rvinowise.ai.unity {

public abstract class Input: MonoBehaviour {

    public Visual_input_receiver receiver;
    public Figure_storage figure_storage;

    

    virtual protected void Awake() {
        figure_storage = receiver.figure_storage;
    }
    
 
}
}