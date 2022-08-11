using rvinowise.ai.general;


namespace rvinowise.ai.ui.general {

public interface IVisual_figure_appearance:
IFigure_appearance,
IAccept_selection
{
    public new IVisual_action get_start();
    public new IVisual_action get_end();
}

}