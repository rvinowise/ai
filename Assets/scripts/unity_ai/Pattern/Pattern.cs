using System.Collections;
using System.Collections.Generic;
using rvinowise.ai.patterns;
using TMPro;
using UnityEditor.UI;
using UnityEngine;

namespace rvinowise.unity.ai.patterns {
public class Pattern : 
MonoBehaviour,
IPattern
{

    public TextMeshPro lable;
    [HideInInspector]
    public Animator animator;

    public bool selected {
        get { return _selected; }
        set {
            _selected = value;
            animator.SetBool("selected", _selected);
        }
    }

    #region IPattern interface
    public string id {
        get { return lable.text; }
        set { lable.text = value; }
    }

    public void add_appearance() {
        animator.SetTrigger("fire");
        _appearances.Add();
    }
    public IReadOnlyList<IPattern_appearance> appearances {
        get => _appearances.AsReadOnly();
    }
    
    #endregion

    private List<IPattern_appearance> _appearances = new List<IPattern_appearance>();

    private bool _selected;

    void Awake() {
        animator = GetComponent<Animator>();
    }
    
    void Start() {
        id = lable.text;
    }

    

   

}
}