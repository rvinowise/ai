using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using abstract_ai;
using rvinowise.ai.patterns;
using rvinowise.rvi.contracts;
using rvinowise.unity.ai.action;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using TMPro;
using UnityEditor.UI;
using UnityEngine;

using Action = rvinowise.unity.ai.action.Action;

namespace rvinowise.unity.ai {
public class Pattern : 
MonoBehaviour,
IPattern,
IHave_destructor
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
            this.set_appearances_are_highlighted(selected);
        }
    }
    
    [SerializeField]
    private List<IPattern_appearance> _appearances = new List<IPattern_appearance>();

    private bool _selected;
    
    #region IPattern interface
    public string id {
        get { return lable.text; }
        set { lable.text = value; }
    }
    public IFigure first_half { get; private set; }
    public IFigure second_half { get; private set; }

    


    [called_by_prefab]
    public Pattern get_for_repeated_pair(
        IFigure beginning,
        IFigure ending
    ) {
        Pattern pattern = this.get_from_pool<Pattern>();
        pattern.id = get_id_for(beginning, ending);
        return pattern; 
    }

    [called_by_prefab]
    public Pattern get_for_base_input(string id) {
        Pattern pattern = this.get_from_pool<Pattern>();
        pattern.id = id;
        pattern.name = string.Format("pattern {0}", pattern.id);
        return pattern;
    }

    public static string get_id_for(IFigure beginning, IFigure ending) {
        string begin = beginning is IPattern beginning_pattern ? beginning_pattern.id : "f";
        string end = beginning is IPattern end_pattern ? end_pattern.id : "f";
        return string.Format("{0}{1}", begin, end);
    }


    IPattern_appearance IPattern.create_appearance(
        BigInteger start,
        BigInteger end
    ) => create_appearance(start, end);


    public IReadOnlyList<IPattern_appearance> appearances {
        get => _appearances.AsReadOnly();
    }
    
    #endregion

    public Pattern_appearance create_appearance(
        BigInteger start,
        BigInteger end
    ) {
        animator.SetTrigger("fire");

        Pattern_appearance appearance =
            pattern_appearance_preafab.get_for_interval(
                this, start, end
            );
        
        
        _appearances.Add(appearance);
        
        return appearance;
    }
    public IPattern_appearance create_appearance(
        IFigure_appearance in_first_half,
        IFigure_appearance in_second_half
    ) {
        animator.SetTrigger("fire");

        Pattern_appearance appearance =
            pattern_appearance_preafab.get_for_subfigures(
                this, in_first_half, in_second_half
            );
        
        
        _appearances.Add(appearance);
        
        return appearance;
    }

    

    void Awake() {
        animator = GetComponent<Animator>();
    }
    
    void Start() {
        id = lable.text;
    }

  

    public IReadOnlyList<IFigure_appearance> get_appearances_in_interval(
        BigInteger start, BigInteger end
    ) {
        List<IPattern_appearance> result = appearances.Where(
            appearance => 
            (appearance.start_moment >= start) &&
            (appearance.end_moment <= end)
        ).ToList<IPattern_appearance>();

        return result.AsReadOnly();
    }

    

    public virtual void destroy() {
        Debug.Log(String.Format("Pattern {0} destroy",id));
        remove_appearances();
        ((MonoBehaviour)this).destroy();
    }

    private void remove_appearances() {
        foreach(var appearance in _appearances) {
            if (appearance is IHave_destructor destructable) {
                destructable.destroy();
            }
        }
        _appearances.Clear();
    }

    void OnMouseDown() {
        this.selected = !selected;
        
    }

    private void set_appearances_are_highlighted(bool value) {
        foreach(var appearance in this.appearances) {
            if (appearance is Pattern_appearance unity_appearance) {
                unity_appearance.selected = value;
            }
        }
    }

    #region IFigure
    public string as_dot_graph() {
        throw new NotImplementedException();
    }

    public IReadOnlyList<IFigure_appearance> get_appearances(IFigure in_where) {
        if (in_where == Action_history.instance) {
            return appearances;
        }
        return null;
    }
    
    #endregion
}
}