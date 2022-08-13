using rvinowise.ai.general;
using UnityEngine;

namespace rvinowise.ai.unity {

public abstract class Input: MonoBehaviour {

    [SerializeField] protected Network network;
    public Visual_input_receiver receiver;
    protected IFigure_provider<Figure> figure_provider;
    

    protected virtual void Awake() {
        figure_provider = network.figure_showcase;
    }
    
 
}
}