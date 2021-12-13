using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using rvinowise;
using rvinowise.rvi.contracts;
using Input = rvinowise.unity.ui.input.Input;
using rvinowise.unity.ai;
using rvinowise.unity.ai.action;
using Action = rvinowise.unity.ai.action.Action;
using rvinowise.ai.patterns;
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