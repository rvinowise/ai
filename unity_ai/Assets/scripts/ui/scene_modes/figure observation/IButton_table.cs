using rvinowise.ai.ui.general;


namespace rvinowise.ai.ui.unity {
public interface IButton_table<out TButton>
    where TButton: class?, new() 
{

    IFigure_button_click_receiver higher_click_receiver { get; set; }

    IFigure_button provide_button_for_figure(IVisual_figure figure);
    IFigure_button create_button_for_figure(IVisual_figure figure);
    IFigure_button get_button_for_figure(IVisual_figure figure);

    void remove_button_for_figure(IVisual_figure figure);
}
}