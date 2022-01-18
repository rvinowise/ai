using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.general;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Linq;
using rvinowise.ai.unity.visuals;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.ui.input;
using rvinowise.unity.ui.input.mouse;
using TMPro;

namespace rvinowise.ai.unity {

public partial class Figure: 
MonoBehaviour,
IFigure,
ICircle,
ISelectable
{
    
    public List<IFigure_representation> representations 
        = new List<IFigure_representation>();

    public List<IFigure_appearance> appearances 
        = new List<IFigure_appearance>();
    
    public Figure_appearance appearance_preafab;
    public Figure_representation representation_prefab;
    
    
    [HideInInspector]
    public Animator animator;
    
    #region building

    public Figure_representation create_representation() {
        Figure_representation representation = representation_prefab.get_from_pool<Figure_representation>();
        representations.Add(representation);
        representation.transform.parent = representations_folder;
        return representation;
    }

    #endregion

    #region IFigure

    public string id {
        get { return label.text; }
        set { label.text = value; }
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

    public TextMeshPro label;
    [SerializeField] private Transform representations_folder;

    void Awake() {
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider>();
        //sprite_renderer  = GetComponent<SpriteRenderer>();
    }

    
    #region ICircle

    public float radius => transform.localScale.x;

    #endregion ICircle

    

    #endregion visualisation



}
}