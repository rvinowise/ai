using System.Collections.Generic;
using rvinowise.ai.general;


namespace rvinowise.ai.ui.general
{

public interface IVisual_figure:
    IFigure,
    IAccept_selection {

    public new IEnumerable<IVisual_figure_appearance> get_appearances();
    public IFigure_button button { get; set; }

    public void show();
    public void hide();
    public bool is_shown { get; }

    public IFigure_header header { get; }

    // public void start_building();
    // public void finish_building();




}
}