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
using rvinowise.unity.ui.input.mouse;

namespace rvinowise.ai.general
{

public interface IVisual_figure:
    IFigure,
    ISelectable
{

    public IFigure_button button { get; set; }

    public void show();
    public void hide();
    public bool is_shown { get; }

    public IFigure_header header { get; }

    // public void start_building();
    // public void finish_building();




}
}