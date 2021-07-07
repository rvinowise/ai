
using UnityEngine;
using TMPro;
using rvinowise.unity.extensions;

namespace rvinowise.unity.ai.action {


public partial class Action: 
MonoBehaviour,
IHave_destructor
{

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

    

    
    public bool highlighted {
        set {
            _highlighted = value;
            animator.SetBool("selected", _highlighted);
            if (value) {
                transform.set_z(transform.parent.position.z - 1);
            } else {
                transform.set_z(transform.parent.position.z - 0.1f);
            }
        }
        get {return _highlighted;}
    }
    private bool _highlighted;
}
}