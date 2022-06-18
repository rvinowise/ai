
using System;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.ai.ui.general;
using rvinowise.ai.unity.visuals;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.extensions;
using TMPro;
using UnityEngine;
using rvinowise.unity.ui.input.mouse;
using rvinowise.ai.unity;
using rvinowise.ai.unity.simple;
using rvinowise.unity.ui.input;

namespace rvinowise.ai.unity
{

public class Subfigure :
    MonoBehaviour,
    ICircular_subfigure,
    ISelectable
{

    #region ISubfigure
    public string id { get; set; }
    public IFigure_representation parent { get; set; }
    public IFigure referenced_figure { get; set; }
    #endregion

    public Stencil_interface direction;

    [called_by_prefab]
    public Subfigure create_for_figure(IFigure figure)
    {
        Subfigure subfigure = this.provide_new<Subfigure>();
        subfigure.id = Id_assigner.get_next_id();
        subfigure.referenced_figure = figure;
        subfigure.init_for_figure(figure);
        return subfigure;
    }
    [called_by_prefab]
    public Subfigure create_for_stencil_interface(Stencil_interface direction)
    {
        Subfigure subfigure = this.provide_new<Subfigure>();
        subfigure.id = Id_assigner.get_next_id();
        subfigure.init_for_stencil(direction);
        return subfigure;
    }

    public IReadOnlyList<ISubfigure> next => _next.AsReadOnly();

    public IReadOnlyList<ISubfigure> previous => _previous.AsReadOnly();


    public List<ISubfigure> _next = new List<ISubfigure>();
    public List<ISubfigure> _previous = new List<ISubfigure>();

    public String get_name()
    {
        return $"{referenced_figure.id}({id})";
    }

    #region building
    public void connext_to_next(ISubfigure next_subfigure)
    {
        _next.Add(next_subfigure);
        next_subfigure.append_previous(this);
        if (next_subfigure is Subfigure unity_subfigure)
        {
            create_connection_arrow_to(unity_subfigure);
        }
    }
    public void append_previous(ISubfigure subfigure)
    {
        _previous.Add(subfigure);
    }
    public void remove_previous(ISubfigure subfigure)
    {
        _previous.Remove(subfigure);
    }

    public bool is_connected(ISubfigure subfigure)
    {
        return _previous.Contains(subfigure) || _next.Contains(subfigure);
    }

    public void disconnect_from_next(ISubfigure disconnectable)
    {
        disconnectable.remove_previous(this);
        _next.Remove(disconnectable);
        if (disconnectable is Subfigure unity_subfigure)
        {
            delete_connection_arrow_to(unity_subfigure);
        }
    }


    private void create_connection_arrow_to(Subfigure next)
    {
        Connection new_connection = connection_prefab.create(this, next);
        new_connection.transform.parent = connections_attachment;
    }
    private void delete_connection_arrow_to(Subfigure next)
    {
        foreach (
            Connection connection in
            connections_attachment.GetComponentsInChildren<Connection>()
        ) {
            if (connection.destination == next) {
                connection.destroy_object();
            }
        }
    }

    #endregion building

    #region visualisation
    [SerializeField] private TextMeshPro lable;
    [SerializeField] private Transform connections_attachment;
    [SerializeField]
    private Connection connection_prefab;

    void Awake() {
        collider = GetComponent<Collider>();
    }

    private void init_for_figure(IFigure figure) {
        lable.text = figure.id;
    }

    private void init_for_stencil(Stencil_interface direction) {
        if (direction == Stencil_interface.input) {
            lable.text = "in";
        } else {
            lable.text = "out";
        }
    }


    public Connection get_connection_to_next(ISubfigure connected_subfigure)
    {
        Connection connection = connections_attachment.transform.
            GetComponentsInChildren<Connection>().
            First(connection => connection.destination == connected_subfigure);
        return connection;
    }

    public float radius => 0.5f;
    public ISubfigure_click_receiver click_receiver;

    
    public IManual_figure_builder manual_figure_builder;

    void OnMouseDown() {
        click_receiver?.on_click(this);
    }
    
    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
        if (collision.transform.GetComponent<Subfigure>() is Subfigure other_subfigure)
        {
            manual_figure_builder.on_subfigures_touched(this, other_subfigure);
        }
    }

    #region ISelectable
    public new Collider collider { get; set; }
    public Renderer selection_sprite_renderer => sprite_renderer;
    [SerializeField]
    private SpriteRenderer sprite_renderer;

    public void accept_selection(Selector selector)
    {
        selector.select(this);
    }
    public void accept_deselection(Selector selector)
    {
        selector.deselect(this);
    }
    #endregion ISelectable
    
    
    
    #endregion visualisation

    
}
}