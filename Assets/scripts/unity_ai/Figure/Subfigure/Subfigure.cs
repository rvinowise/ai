
using System;
using System.Collections.Generic;
using abstract_ai;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.extensions;
using TMPro;
using UnityEngine;

namespace rvinowise.unity.ai.figure {

public class Subfigure:
MonoBehaviour,
ISubfigure 
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
    }
    public void append_next(ISubfigure subfigure) {
        _next.Add(subfigure);
    }
    public void append_previous(ISubfigure subfigure) {
        _previous.Add(subfigure);
    }

    #endregion

    #region visualisation
    [SerializeField]
    private TextMeshPro lable; 
    void Awake() {
        lines_to_next = GetComponent<LineRenderer>();
    }

    private void set_appearance_for_figure(IFigure figure) {
        lable.text = figure.id;
    }
    private void update_connections() {
        
    }
    #endregion
}
}