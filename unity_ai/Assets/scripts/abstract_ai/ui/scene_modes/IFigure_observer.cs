namespace rvinowise.ai.ui.general {

public interface IFigure_observer : 
IFigure_button_click_receiver 
{
    public IVisual_figure observed_figure { get; }


    public void observe(IVisual_figure figure);

    public void finish_observing();

    





    public void on_click(IFigure_button figure_button);



}
}