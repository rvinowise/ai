using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using rvinowise.ai.patterns;
using rvinowise.rvi.contracts;
using rvinowise.unity.ai.action;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using TMPro;
using UnityEditor.UI;
using UnityEngine;

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

    #region IPattern interface
    public string id {
        get { return lable.text; }
        set { lable.text = value; }
    }

    public IPattern beginning {get;private set;}
    public IPattern ending {get;private set;}

    [called_by_prefab]
    public Pattern get_for_appearances(
        List<IPattern_appearance> appearances
    ) {
        Contract.Requires(appearances.Count > 1);
        Pattern pattern = this.get_for_repeated_pair(
            appearances.First().start.pattern,
            appearances.First().end.pattern
        );
        pattern._appearances = appearances;
        return pattern;
    }

    [called_by_prefab]
    public Pattern get_for_repeated_pair(
        IPattern beginning,
        IPattern ending
    ) {
        Pattern pattern = this.get_from_pool<Pattern>();
        pattern.beginning = beginning; 
        pattern.ending = ending;
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

    public string get_id_for(IPattern beginning, IPattern ending) {
        return string.Format("{0}{1}",beginning.id, ending.id);
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

        Pattern_appearance appearance =
            pattern_appearance_preafab.get_for_interval(
                this, start_group, end_group
            );
        
        _appearances.Add(appearance);
        
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

  

    public IReadOnlyList<IPattern_appearance> get_appearances_in_interval(
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
        foreach(var appearance in _appearances) {
            if (appearance is IHave_destructor destructable) {
                destructable.destroy();
            }
        }
        ((MonoBehaviour)this).destroy();
    }

    void OnMouseDown() {
        this.selected = !selected;
        
    }

    private void set_appearances_are_highlighted(bool value) {
        foreach(var appearance in this.appearances) {
            if (appearance.start is Action unity_start) {
                unity_start.highlighted = value;
            }
            if (appearance.end is Action unity_end) {
                unity_end.highlighted = value;
            }
        }
    }
}
}