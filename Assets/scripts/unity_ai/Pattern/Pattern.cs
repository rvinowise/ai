using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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

    public Pattern_appearance pattern_appearance_preafab;

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

    IPattern_appearance IPattern.create_appearance(
        IAction_group start_group,
        IAction_group end_group
    ) => create_appearance(start_group, end_group);
    
    public IReadOnlyList<IPattern_appearance> appearances {
        get => _appearances.AsReadOnly();
    }
    
    #endregion

    public Pattern_appearance create_appearance(
        IAction_group start_group,
        IAction_group end_group
    ) {
        animator.SetTrigger("fire");
        //_appearances.Add();

        Pattern_appearance appearance =
            pattern_appearance_preafab.get_for_interval(
                this, start_group, end_group
            );
        return appearance;
    }

    private List<IPattern_appearance> _appearances = new List<IPattern_appearance>();

    private bool _selected;

    void Awake() {
        animator = GetComponent<Animator>();
    }
    
    void Start() {
        id = lable.text;
    }

  

    public IReadOnlyList<IPattern_appearance> get_appearances_in_interval(BigInteger start, BigInteger end)
    {
        throw new System.NotImplementedException();
    }
}
}