
using System;
using System.Collections.Generic;
using abstract_ai;
using rvinowise.unity.ai.visuals;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.extensions;
using TMPro;
using UnityEngine;
using rvinowise.unity.ui.input.mouse;
using rvinowise.unity.ui.input;

namespace rvinowise.unity.ai.figure {

public class Subfigure:
MonoBehaviour,
ISubfigure,
ICircle,
ISelectable
{

    #region ISubfigure
    public IFigure parent {get;set;}
    public IFigure figure {get;set;}
    #endregion

    [called_by_prefab]
    public Subfigure create_for_figure(IFigure figure) {
        Subfigure subfigure = this.get_from_pool<Subfigure>();
        subfigure.figure = figure;
        subfigure.set_appearance_for_figure(figure);
        return subfigure;
    }
    //public Figure unity_parent { get; private set; }
    //public Figure unity_figure {get; private set; }

    public IReadOnlyList<ISubfigure> next { get{
        return _next.AsReadOnly();
    }}
    
    public IReadOnlyList<ISubfigure> previous { get{
        return _previous.AsReadOnly();
    }}

    private List<ISubfigure> _next = new List<ISubfigure>();
    private List<ISubfigure> _previous = new List<ISubfigure>();

    public string id;

    private LineRenderer lines_to_next;
    

    public String get_name() {
        return String.Format("{0}({1})", figure.id, id);
    }
    
    #region building
    public void connext_to_next(ISubfigure next_subfigure) {
        append_next(next_subfigure);
        next_subfigure.append_previous(this);
        if (next_subfigure is Subfigure unity_subfigure) {
            create_connection_arrow_to(unity_subfigure);
        }
    }
    public void append_next(ISubfigure subfigure) {
        _next.Add(subfigure);
    }
    public void append_previous(ISubfigure subfigure) {
        _previous.Add(subfigure);
    }

    private void create_connection_arrow_to(Subfigure next) {
        Connection new_connection = connection_prefab.create(this, next);
        new_connection.transform.parent = connections_folder;
    }

    #endregion

    #region visualisation
    [SerializeField]
    private TextMeshPro lable;
    [SerializeField]
    private Transform connections_folder;
    [SerializeField]
    private Connection connection_prefab;
    
    void Awake() {
        lines_to_next = GetComponent<LineRenderer>();
        collider = GetComponent<Collider>();
    }

    private void set_appearance_for_figure(IFigure figure) {
        lable.text = figure.id;
    }
    private void update_connections() {
        
    }

    public float radius => 0.5f;


    #region ISelectable
    public new Collider collider{get;set;}
    public bool selected{
        set {
            _selected = value;
            
        }
        get => _selected;
    }
    private bool _selected;
    public SpriteRenderer selection_sprite_renderer => sprite_renderer;
    [SerializeField]
    private SpriteRenderer sprite_renderer; 
    #endregion
    #endregion
}
}