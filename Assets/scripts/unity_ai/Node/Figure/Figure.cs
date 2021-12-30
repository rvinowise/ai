using System.Collections.Generic;
using System.Numerics;
using abstract_ai;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Linq;
using rvinowise.unity.ai.visuals;

namespace rvinowise.unity.ai.figure {

public class Figure: 
MonoBehaviour,
IFigure,
ICircle
{
    
    public List<ISubfigure> first_subfigures = new List<ISubfigure>();

    public List<ISubfigure> subfigures = new List<ISubfigure>();

    public bool selected {
        get { return _selected; }
        set {
            _selected = value; 
            animator.SetBool("selected", _selected);
            //this.set_appearances_are_highlighted(selected);
        }
    }
    private bool _selected = false;
    [HideInInspector]
    public Animator animator;
    
    #region building
    public Subfigure add_subfigure(IFigure child_figure) {
        Subfigure subfigure = subfigure_prefab.
            create_for_figure(child_figure);
        subfigure.transform.parent = subfigures_folder.transform;
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

    public IReadOnlyList<IFigure_appearance> get_appearances_in_interval(BigInteger start, BigInteger end) {
        throw new System.NotImplementedException();
    }
    #endregion IFigure
    
    #region visualisation
    [SerializeField]
    public Subfigure subfigure_prefab;
    public Transform subfigures_folder;
    private Vector3 subfigures_offset = new Vector3(2,0,0);
    void Awake() {
        animator = GetComponent<Animator>();
    }
    void OnMouseDown() {
        selected = !selected;
    }

    private void position_subfigure(Subfigure subfigure) {
        
        subfigure.transform.position = 
            get_position_of_last_subfigure()+ subfigures_offset;
    }

    private Vector3 get_position_of_last_subfigure() {
        if (subfigures.Any()) {
            var last_subfigue = subfigures.Last() as Subfigure;
            return last_subfigue.transform.position;
        }
        return transform.position + new Vector3(0,-2,0);
    }
    #region ICircle

    public float radius => transform.localScale.x;

    #endregion

    #endregion



}
}