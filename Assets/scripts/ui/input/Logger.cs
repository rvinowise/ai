using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using rvinowise;
using rvinowise.rvi.contracts;
using rvinowise.ai.unity;
using rvinowise.ai.unity;
using Action = rvinowise.ai.unity.Action;
using rvinowise.ai.general;
using rvinowise.unity.geometry2d;
using UnityEngine.Assertions;

namespace rvinowise.unity.ui {



public class Logger : MonoBehaviour {
    
    private static Logger instance;
    
    void Awake() {
        Contract.Assert(instance == null, "singleton");
        instance = this;
    }
    


}
}