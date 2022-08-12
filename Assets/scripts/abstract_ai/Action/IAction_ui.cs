

using rvinowise.ai.ui.general;

namespace rvinowise.ai.general {

public interface IAction_ui:
IAccept_selection
{
    
    // visual things should have setters for most fields, because unity doesn't have constructors for prefabs
    // IFigure_appearance figure_appearance { get; set; }
    // IAction_group action_group { get; set; }
    // void set_label(string text);
    
    Selection_of_object selection { get; }
}
}