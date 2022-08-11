using rvinowise.ai.general;
using rvinowise.ai.ui.general;


namespace rvinowise.ai.ui.unity {
public interface IFigure_showcase<out TFigure>:
    IFigure_provider<TFigure>
    where TFigure: class?, IFigure, new() 
{

    
    public IVisual_figure shown_figure { get; }

    public void show_insides_of_one_figure(IVisual_figure new_shown_figure);

    #region testing

    IFigure_button get_button_for_figure(IVisual_figure figure);

    #endregion testing
    
}
}