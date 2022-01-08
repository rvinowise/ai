using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.general;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Linq;
using rvinowise.ai.unity.visuals;
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
        collider = GetComponent<Collider>();
        //sprite_renderer  = GetComponent<SpriteRenderer>();
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
    #endregion

    #endregion



}
}