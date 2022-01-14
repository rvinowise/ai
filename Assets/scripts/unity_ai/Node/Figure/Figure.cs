using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.general;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Linq;
using rvinowise.ai.unity.visuals;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.ui.input.mouse;

namespace rvinowise.ai.unity {

public class Figure: 
MonoBehaviour,
IFigure,
ICircle,
ISelectable
{
    
    public List<ISubfigure> first_subfigures = new List<ISubfigure>();
    public List<ISubfigure> subfigures = new List<ISubfigure>();

    public List<Figure_representation> representations 
        = new List<Figure_representation>();

    public List<IFigure_appearance> appearances 
        = new List<IFigure_appearance>();
    
    public Pattern_appearance appearance_preafab;
    
    
    [HideInInspector]
    public Animator animator;
    
    public override int GetHashCode() {
        return base.GetHashCode();
    }
    
    #region building
    public Subfigure add_subfigure(IFigure child_figure) {
        Subfigure subfigure = subfigure_prefab.
            create_for_figure(child_figure);
        subfigure.transform.parent = subfigures_folder.transform;
        subfigure.parent = this;
        position_subfigure(subfigure);
        subfigures.Add(subfigure);
        return subfigure;
    }
    
    #endregion

    #region IFigure

    public string id { get; set; }

    public string as_dot_graph() {
        throw new System.NotImplementedException();
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

    public void add_appearance(IFigure_appearance appearance) {
        if (animator != null) {
            animator.SetTrigger("fire");
        }
        appearances.Add(appearance);
    }
    #endregion IFigure
    
    
    #region sequential figure
    public List<IFigure> sequence = new List<IFigure>();
    public IReadOnlyList<IFigure> as_lowlevel_sequence() {
        if (sequence.Any()) {
            return sequence.AsReadOnly();
        }
        return new List<IFigure> {this};
    }

    public bool is_sequential() {
        return sequence.Any();
    }
    #endregion sequential figure

    


    #region visualisation

    void Awake() {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        //sprite_renderer  = GetComponent<SpriteRenderer>();
    }

    
    #region ICircle

    public float radius => transform.localScale.x;

    #endregion ICircle

    #region ISelectable
    public bool selected {
        get { return _selected; }
        set {
            _selected = value; 
            // if (value) {
            //     sprite_renderer.color = new Color(1,0,0);
            // } else {
            //     sprite_renderer.color = new Color(1,1,1);
            // }
            //this.set_appearances_are_highlighted(selected);
        }
    }
    private bool _selected = false;
    [SerializeField]
    public SpriteRenderer selection_sprite_renderer => sprite_renderer;
    [SerializeField]
    private SpriteRenderer sprite_renderer;
    public new Collider collider{get;private set;}
    #endregion ISelectable

    #endregion visualisation



}
}