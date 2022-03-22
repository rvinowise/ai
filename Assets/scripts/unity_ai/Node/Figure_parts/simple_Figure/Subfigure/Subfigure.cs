
using System;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.ai.unity.visuals;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.extensions;
using TMPro;
using UnityEngine;
using rvinowise.unity.ui.input.mouse;
using rvinowise.ai.unity;
using rvinowise.unity.ui.input;

namespace rvinowise.ai.unity.simple
{


public enum Stencil_interface {
    input= 0,
    output=1
}

public class Subfigure:
    ISubfigure
{

    #region ISubfigure
    public string id { get; set; }
    public IFigure_representation parent { get; set; }
    public IFigure referenced_figure { get; set; }
    #endregion

    public Stencil_interface direction;

    public static Subfigure create_for_figure(IFigure figure) {
        Subfigure subfigure = new Subfigure {
            id = Id_assigner.get_next_id(), 
            referenced_figure = figure
        };
        return subfigure;
    }
    public static Subfigure create_for_stencil_interface(Stencil_interface direction) {
        Subfigure subfigure = new Subfigure {
            id = Id_assigner.get_next_id()
        };
        return subfigure;
    }

    public IReadOnlyList<ISubfigure> next => _next.AsReadOnly();

    public IReadOnlyList<ISubfigure> previous => _previous.AsReadOnly();


    private List<ISubfigure> _next = new List<ISubfigure>();
    private List<ISubfigure> _previous = new List<ISubfigure>();

    public String get_name()
    {
        return $"{referenced_figure.id}({id})";
    }

    #region building
    public void connext_to_next(ISubfigure next_subfigure)
    {
        _next.Add(next_subfigure);
        next_subfigure.append_previous(this);
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
    }



    #endregion building

    
}
}