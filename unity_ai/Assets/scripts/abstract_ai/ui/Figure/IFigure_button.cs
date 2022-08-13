using rvinowise.ai.unity.simple;
using rvinowise.unity.extensions.attributes;


namespace rvinowise.ai.ui.general {


public interface IFigure_button:
IAccept_selection
{

    IVisual_figure figure { get; }
    IFigure_button_click_receiver click_receiver { get; set; }

    [called_by_prefab]
    public IFigure_button create_for_figure(IVisual_figure figure);

    [called_by_prefab]
    public IFigure_button create_for_stencil_node(Stencil_interface direction);
    
    //inspector event
    public void on_click();

}
}