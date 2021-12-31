using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using rvinowise;
using rvinowise.rvi.contracts;
using Input = rvinowise.unity.ui.input.Input;


namespace rvinowise.unity.ui.input.mouse {

public interface ISelectable {
    Transform transform{get;}
    Collider collider {get;}
    bool selected{get;set;}
}
}