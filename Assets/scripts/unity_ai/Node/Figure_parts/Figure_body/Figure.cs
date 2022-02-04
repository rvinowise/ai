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
IFigure
{
    
    public List<IFigure_representation> representations 
        = new List<IFigure_representation>();

    public List<IFigure_appearance> _appearances 
        = new List<IFigure_appearance>();
    
    public Figure_appearance appearance_preafab;
    public Figure_representation representation_prefab;
    
    
    [HideInInspector]
    public Animator animator;
    
    #region building

    public IFigure_representation create_representation() {
        Figure_representation representation = representation_prefab.provide_new<Figure_representation>();
        representations.Add(representation);
        representation.transform.SetParent(representations_folder,false);
        return representation;
    }

    #endregion

    #region IFigure

    public string id {
        get { return lable.text; }
        set { lable.text = value; }
    }
    public IReadOnlyList<IFigure_appearance> get_appearances() => _appearances;

    public IReadOnlyList<IFigure_appearance> get_appearances_in_interval(
        BigInteger start, BigInteger end
    ) {
        return _appearances.Where(
            appearance => 
                (appearance.start_moment >= start) &&
                (appearance.end_moment <= end)
        ).ToList().AsReadOnly();
    }

    public void add_appearance(IFigure_appearance appearance) {
        if (animator != null) {
            animator.SetTrigger("fire");
        }
        _appearances.Add(appearance);
        if (appearance is Figure_appearance unity_appearance) {
            unity_appearance.transform.parent = this.transform;
            unity_appearance.transform.localPosition = Vector3.zero;
        }
    }

    public IReadOnlyList<IFigure_representation> get_representations() 
        => representations.AsReadOnly();
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

    public TMP_Text lable;
    [SerializeField] private Transform representations_folder;
    public Figure_button button;
    void Awake() {
        collider = GetComponent<Collider>();
    }

    public void show_inside() {
        gameObject.SetActive(true);
    }
    public void hide_inside() {
        gameObject.SetActive(false);
    }

    [SerializeField] public Figure_header header; 
    public void start_building() {
        header.start_building();
    }
    public void finish_building() {
        header.finish_building();
    }

    #endregion visualisation

    #region IDestructable

    public void destroy() {
        //base.destroy();
        foreach (Figure_appearance appearance in get_appearances()) {
            appearance.destroy();
        }
        this.destroy_object();
    }
    #endregion IDestructable

}
}