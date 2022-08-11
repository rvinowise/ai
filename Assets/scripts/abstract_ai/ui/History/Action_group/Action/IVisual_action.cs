using rvinowise.ai.general;


namespace rvinowise.ai.ui.general {

public interface IVisual_action: 
    IAction,
    IAccept_selection,
    IHave_destructor
{
    // visual things should have setters for most fields, because unity doesn't have constructors for prefabs
    new IFigure_appearance figure_appearance { get; set; }
    new IAction_group action_group { get; set; }
    void set_label(string text);
}

}