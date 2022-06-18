using rvinowise.ai.general;
using rvinowise.ai.unity;
using rvinowise.ai.unity.simple;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.ui.input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace rvinowise.ai.ui.general {


public interface IFigure_button {

    IVisual_figure figure { get; }
    IFigure_button_click_receiver click_receiver { get; set; }

    [called_by_prefab]
    public IFigure_button create_for_figure(IVisual_figure figure);

    [called_by_prefab]
    public IFigure_button create_for_stencil_node(Stencil_interface direction);
    
    //inspector event
    public void on_click();


    public void highlight_as_selected();

    public void dehighlight_as_selected();
}
}