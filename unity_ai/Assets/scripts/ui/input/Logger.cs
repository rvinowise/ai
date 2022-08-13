using UnityEngine;
using rvinowise.contracts;


namespace rvinowise.unity.ui {



public class Logger : MonoBehaviour {
    
    private static Logger instance;
    
    void Awake() {
        Contract.Assert(instance == null, "singleton");
        instance = this;
    }
    


}
}