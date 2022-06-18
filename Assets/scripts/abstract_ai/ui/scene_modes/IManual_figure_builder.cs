
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.ai.ui.unity;
using rvinowise.ai.unity.simple;
using rvinowise.rvi.contracts;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.ui.input;
using rvinowise.unity.ui.input.mouse;
using UnityEngine.EventSystems;
using rvinowise.unity.geometry2d;
using Vector3 = UnityEngine.Vector3;
using Input = UnityEngine.Input;
using Subfigure = rvinowise.ai.unity.Subfigure;

namespace rvinowise.ai.ui.general {


public interface IManual_figure_builder:
    IFigure_button_click_receiver,
    ISubfigure_click_receiver 
{
    public bool change_connections { get; set; }
    public IFigure figure { get; } 

    public void on_create_empty_figure();

    public void on_start_editing_figure(IFigure figure);

    public void deactivate();

    public void on_click(IFigure_button figure_button);
    public void on_click_stencil_interface(Stencil_interface direction);

    public void on_click(ISubfigure subfigure);
    public void deselect_all();

    public void on_subfigures_touched(Subfigure moved_subfigure, Subfigure other_subfigure);
}
}