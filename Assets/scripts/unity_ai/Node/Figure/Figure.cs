using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.general;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Linq;
using rvinowise.ai.ui.general;
using rvinowise.unity.extensions;
using TMPro;
using rvinowise.unity;

namespace rvinowise.ai.unity
{

public partial class Figure: 
    MonoBehaviour,
    IVisual_figure,
    IHave_destructor
{
    
    public readonly List<IFigure_representation> representations 
        = new List<IFigure_representation>();

    public readonly List<IFigure_appearance> _appearances 
        = new List<IFigure_appearance>();
    
    #region unity inspector
    
    public Figure_appearance appearance_preafab;
    public Figure_representation representation_prefab;
    public Figure_header _header;
    
    public TMP_Text lable;
    [SerializeField] private Transform representations_folder;
    
    #endregion unity inspector
    
    #region building

    public IFigure_representation create_representation() {
        Figure_representation representation = representation_prefab.provide_new<Figure_representation>();
        representations.Add(representation);
        representation.transform.SetParent(representations_folder,false);
        return representation;
    }

    #endregion building

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

        _appearances.Add(appearance);
        if (appearance is Figure_appearance unity_appearance) {
            unity_appearance.transform.parent = this.transform;
            unity_appearance.transform.localPosition = Vector3.zero;
        }
    }

    public IReadOnlyList<IFigure_representation> get_representations() 
        => representations.AsReadOnly();
    
    #region IVisual_figure

    public IFigure_button button { get; set; }
    
    public IFigure_header header {
        get => _header;
    }
    
    public void show() {
        gameObject.SetActive(true);
    }
    public void hide() {
        gameObject.SetActive(false);
    }

    public bool is_shown => gameObject.activeSelf; 

    #endregion IVisual_figure
    
    #endregion IFigure
    
    
    #region sequential figure
    public void set_lowlevel_sequence(IEnumerable<IFigure> in_sequence) {
        foreach(IFigure figure in in_sequence){
            sequence.Add(figure); 
        }
    }
    private List<IFigure> sequence = new List<IFigure>();
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


    void Awake() {
        collider = GetComponent<Collider>();
    }

    #region IDestructable

    public void destroy() {
        //base.destroy();
        foreach (IFigure_appearance appearance in get_appearances()) {
            if (appearance is IHave_destructor destructable_appearance) {
                destructable_appearance.destroy();
            }
        }
        this.destroy_object();
    }
    #endregion IDestructable

}
}