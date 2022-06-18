



namespace rvinowise.ai.ui.general {


public interface IMode_selector {

    public void on_start_building_figure();
    public void on_start_editing_figure();

    public void on_start_editing_figure_without_changing_connections();

    public void on_finish_building_figure();

    public void stop_observing_figure();


}
}