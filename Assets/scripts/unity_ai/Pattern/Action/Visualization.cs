
using UnityEngine;
using TMPro;

namespace rvinowise.unity.ai.action {


public partial class Action: MonoBehaviour {

    [SerializeField]
    private TextMeshPro lable;
    [HideInInspector]
    public Animator animator;
    
    void Awake() {
        animator = GetComponent<Animator>();
    }
    public void set_label(string in_text) {
        lable.text = in_text;
    }
}
}