using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.ai.unity;
using rvinowise.ai.unity.visuals;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.ai.unity.persistence;
using rvinowise.unity.ui.input.mouse;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using rvinowise.unity;

namespace rvinowise.ai.unity {
public class Pattern : 
MonoBehaviour,
IPattern,
IHave_destructor,
ICircle,
ISelectable
{

    public TextMeshPro label;
    [HideInInspector]
    public Animator animator;

    public Pattern_appearance pattern_appearance_preafab;

    
    public List<IFigure_appearance> appearances = new List<IFigure_appearance>();
    
    public IReadOnlyList<IFigure> subfigures { get; private set; } 
        = new List<IFigure>();

    [SerializeField] //debug
    private bool _selected;
    
    #region IPattern

    #region IFigure
    public string id {
        get { return label.text; }
        set { label.text = value; }
    }
    public string as_dot_graph() {
        throw new NotImplementedException();
    }
    public IReadOnlyList<IFigure_appearance> get_appearances_in_interval(
        BigInteger start, BigInteger end
    ) {
        return appearances.Where(
            appearance => 
            (appearance.start_moment >= start) &&
            (appearance.end_moment <= end)
        ).ToList().AsReadOnly();
    }

    #endregion IFigure

    public void add_appearance(
        IFigure_appearance appearance
    ) {
        if (animator != null) {
            animator.SetTrigger("fire");
        }

        appearances.Add(appearance);
    }

    public IReadOnlyList<IFigure> as_lowlevel_sequence() {
        if (subfigures.Any()) {
            return subfigures;
        }
        return new List<IFigure> {this};
    }
    
    #endregion IPattern

    
    
    

    [called_by_prefab]
    public Pattern get_for_sequence_of_subfigures(
        IReadOnlyList<IFigure> subfigures
    ) {
        Pattern pattern = this.get_from_pool<Pattern>();

        pattern.subfigures = subfigures;
        pattern.id = get_id_for(subfigures);

        return pattern;
    }

    public static IReadOnlyList<IFigure> get_sequence_of_subfigures_from(
        IFigure beginning, IFigure ending
    ) {
        return get_sequence_of_subfigures_from(beginning).Concat(
            get_sequence_of_subfigures_from(ending)
        ).ToList();
    }
    
    private static IReadOnlyList<IFigure> get_sequence_of_subfigures_from(IFigure figure) {
        if (figure is IPattern pattern) {
            if (pattern.subfigures.Any()) {
                return pattern.subfigures; 
            }
        }
        return new List<IFigure>{figure};
    }

    [called_by_prefab]
    public Pattern get_for_base_input(string id) {
        Pattern pattern = this.get_from_pool<Pattern>();
        pattern.id = id;
        pattern.name = string.Format("pattern {0}", pattern.id);
        return pattern;
    }


    public static string get_id_for(IReadOnlyList<IFigure> subfigures) {
        StringBuilder res = new StringBuilder();
        foreach (var subfigure in subfigures) {
            res.Append(subfigure.id);
        }

        return res.ToString();
    }

    void Awake() {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
    }

    void Start() {
        id = label.text;
    }

    public virtual void destroy() {
        Debug.Log(String.Format("Pattern {0} destroy",id));
        remove_appearances();
        ((MonoBehaviour)this).destroy();
    }

    private void remove_appearances() {
        foreach(var appearance in appearances) {
            if (appearance is IHave_destructor destructable) {
                destructable.destroy();
            }
        }
        appearances.Clear();
    }

    private void set_appearances_are_highlighted(bool value) {
        foreach(var appearance in this.appearances) {
            if (appearance is Pattern_appearance unity_appearance) {
                unity_appearance.selected = value;
            }
        }
    }

    #region visualisation
    #region ICircle
    public float radius => transform.localScale.x;
    #endregion

    #region ISelectable
    public bool selected {
        get { return _selected; }
        set {
            _selected = value;
            //animator.SetBool("selected", _selected);
            this.set_appearances_are_highlighted(selected);
        }
    }
    public SpriteRenderer selection_sprite_renderer => sprite_renderer;
    [SerializeField]
    private SpriteRenderer sprite_renderer;
    public new Collider collider{get;set;}
    #endregion
    #endregion

    
}
}