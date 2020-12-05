using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace rvinowise.unity.ai.patterns {


public class Signal : MonoBehaviour {

    #region visuals
    public float visual_spacing = 2f;
    [SerializeField]
    private TextMeshPro lable;
    [HideInInspector]
    public Animator animator;
    #endregion
    
    public Pattern pattern;
    public IList<Signal> next_signals;
    public IList<Signal> prev_signals;
    public long index;
    
    void Awake() {
        animator = GetComponent<Animator>();
    }
    public void init_for_pattern(Pattern in_pattern) {
        pattern = in_pattern;
        set_label(in_pattern.id);
    }

    public void set_label(string in_text) {
        lable.text = in_text;
    }
    
}

}