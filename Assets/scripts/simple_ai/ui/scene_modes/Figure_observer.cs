using rvinowise.ai.ui.general;
using UnityEngine;
using rvinowise.ai.unity;
using rvinowise.ai.unity.simple;
using rvinowise.unity.ui.input;
using Action = rvinowise.ai.unity.Action;
using rvinowise.unity.ui.input.mouse;

namespace rvinowise.ai.ui.simple
{

public class Figure_observer : 
MonoBehaviour,
IFigure_button_click_receiver 
{

    [SerializeField] private Figure_showcase figure_showcase;
    [SerializeField] private Selector selector; 
    public Figure observed_figure;
    public Color selected_color = new Color(0,1,0);
    public Color normal_color = new Color(1,1,1);
    public Color highlighted_color = new Color(0.9f,1,0.9f);


    public void observe(Figure figure) {
        observed_figure = figure;
        mark_object_as_selected(figure);
        figure.show();
        figure.button.highlight_as_selected();
        foreach (Figure_appearance appearance in figure.get_appearances()) {
            highlight(appearance);
        }
    }

    public void activate() {
        enabled = true;
        figure_showcase.receiver = this;
    }
    
    
    public void deactivate() {
        finish_observing();
        enabled = false;
    }
    
    public void finish_observing() {
        if (!observed_figure) {
            return;
        }
        mark_object_as_deselected(observed_figure);
        observed_figure.hide();
        observed_figure.button.dehighlight_as_selected();
        foreach (Figure_appearance appearance in observed_figure._appearances) {
            dehighlight(appearance);
        }
        observed_figure = null;
        
    }
    

    
    public void highlight(Figure_appearance appearance) {
        highlight_generally(appearance);
        appearance.bezier.gameObject.SetActive(true);
        highlight(appearance.start_action);
        highlight(appearance.end_action);
    }
    public void dehighlight(Figure_appearance appearance) {
        dehighlight_generally(appearance);
        appearance.bezier.gameObject.SetActive(false);
        dehighlight(appearance.start_action);
        dehighlight(appearance.end_action);
    }
    
    
    public void highlight(Action action) {
        highlight_generally(action);
    }

    public void dehighlight(Action action) {
        dehighlight_generally(action);
    }
    
    private void highlight_generally(ISelectable highlightable) {
        if (highlightable.selection_sprite_renderer!=null) {
            highlightable.selection_sprite_renderer.material.color = highlighted_color;
        }
    }
    private void dehighlight_generally(ISelectable highlightable) {
        if (highlightable.selection_sprite_renderer!=null) {
            highlightable.selection_sprite_renderer.material.color = normal_color;
        }
    }
    
   
    private void mark_object_as_selected(ISelectable selectable) {
        if (selectable.selection_sprite_renderer!=null) {
            selectable.selection_sprite_renderer.material.color = selected_color;
        }
    }
    private void mark_object_as_deselected(ISelectable selectable) {
        if (selectable.selection_sprite_renderer!=null) {
            selectable.selection_sprite_renderer.material.color = normal_color;
        }
    }



    public void on_click(IFigure_button figure_button) {
        finish_observing();
        selector.deselect_all_figures();
        if (figure_button.figure is Figure figure) {
            observe(figure);
            selector.select(figure);
        }
    }


    public void on_click_stencil_interface(Stencil_interface direction) {
    }


}
}