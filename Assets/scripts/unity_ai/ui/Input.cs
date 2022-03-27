using System.Collections.Generic;
using rvinowise.ai.general;
using UnityEngine;

namespace rvinowise.ai.unity {

public abstract class Input: MonoBehaviour {

    public Visual_input_receiver receiver;
    public Figure_showcase figure_showcase;

    

    protected virtual void Awake() {
        figure_showcase = receiver.figure_showcase;
    }
    
 
}
}