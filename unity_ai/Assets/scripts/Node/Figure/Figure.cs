using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.general;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Linq;
using rvinowise.ai.ui.general;
using rvinowise.contracts;
using rvinowise.unity.extensions;
using TMPro;


namespace rvinowise.ai.unity
{

public class Figure: 
    MonoBehaviour,
    IVisual_figure,
    IHave_destructor
{
    
    public readonly List<IFigure_representation> representations = new();

    private readonly List<IVisual_figure_appearance> _appearances = new();
    
    #region unity inspector
    
    public Figure_appearance appearance_preafab;
    public Figure_representation representation_prefab;
    public Figure_header _header;
    
    public TMP_Text label;
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
        get => label.text;
        set => label.text = value;
    }
    IEnumerable<IFigure_appearance> IFigure.get_appearances() => _appearances.AsReadOnly();
    public IEnumerable<IVisual_figure_appearance> get_appearances() => _appearances.AsReadOnly();

    public IReadOnlyList<IFigure_appearance> get_appearances_in_interval(
    BigInteger start, BigInteger end
    ) {
        return _appearances.Where(
            appearance => 
                (appearance.start_moment >= start) &&
                (appearance.end_moment <= end)
        ).ToList().AsReadOnly();
    }

    void IFigure.add_appearance(IFigure_appearance appearance) {
        Contract.Ensures(
            appearance is IVisual_figure_appearance,
            "Visual figure can only have visual appearances, but a simple one was provided"
        );
        if (appearance is IVisual_figure_appearance visual_appearance) {
            _appearances.Add(visual_appearance);
            if (appearance is Figure_appearance unity_appearance) {
                unity_appearance.transform.parent = transform;
                unity_appearance.transform.localPosition = Vector3.zero;
            }
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

    #region IAccept_selection

    private void Awake() {
        figure_selection = new Figure_selection(GetComponent<SpriteRenderer>());
    }
    public Selection_of_object selection => figure_selection;
    private Figure_selection figure_selection;

    #endregion IAccept_selection
    
    #endregion IVisual_figure
    
    #endregion IFigure
    
    
    #region sequential figure
    public void set_lowlevel_sequence(IEnumerable<IFigure> in_sequence) {
        foreach(IFigure figure in in_sequence){
            sequence.Add(figure); 
        }
    }
    private List<IFigure> sequence = new();
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