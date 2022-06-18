using rvinowise.ai.unity.simple;

namespace rvinowise.ai.ui.general {


public interface IFigure_button_click_receiver {
    void on_click(IFigure_button figure_button);
    void on_click_stencil_interface(Stencil_interface direction);
}

}