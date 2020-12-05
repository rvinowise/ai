/* visualises all the signals that were input into the system */

using System;
using System.Collections.Generic;
using UnityEngine;
using rvinowise.unity.extensions;

namespace rvinowise.unity.ai.patterns {

public class Signal_history : 
    Input_receiver
{

    
    public Signal signal_prefab;

    private Signal last_signal;
    private Vector3 carret;
    private Vector3 signal_step;

    public List<Pattern> patterns;


    void Awake() {
        carret = new Vector2(0f,0f);
    }
    void Start() {
        signal_step = new Vector2(signal_prefab.visual_spacing,0f);
    }
    public override void input_pattern(Pattern in_pattern) {
        Signal new_signal = signal_prefab.get_from_pool<Signal>();
        new_signal.init_for_pattern(in_pattern);
        place_next_signal(new_signal);
        in_pattern.add_appearance();
        new_signal.animator.SetTrigger("fire");
    }

    public override void input_selected_patterns() {
        throw new NotImplementedException();
    }



    private void place_next_signal(Signal in_signal) {
        in_signal.transform.position = carret;
        carret += signal_step;
            /*carret.transform.position +
            signal_step;*/
    }

    
}
}