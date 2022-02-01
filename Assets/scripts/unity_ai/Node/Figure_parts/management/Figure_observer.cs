using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using rvinowise;
using rvinowise.rvi.contracts;
using rvinowise.ai.unity;
using Action = rvinowise.ai.unity.Action;
using rvinowise.ai.general;
using rvinowise.unity.geometry2d;
using UnityEngine.Assertions;
using rvinowise.unity.ui.input.mouse;
using rvinowise.ai.unity;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Selectable = rvinowise.unity.ui.input.mouse.Selectable;

namespace rvinowise.unity.ui.input {

public class Figure_observer : 
MonoBehaviour,
IFigure_button_click_receiver {

    [SerializeField] private Figure_storage figure_storage;
    private Figure observed_figure;
    public Color selected_color = new Color(0,1,0);
    public Color normal_color = new Color(1,1,1);
    public Color highlighted_color = new Color(0.9f,1,0.9f);



    public void observe(Figure figure) {
        observed_figure = figure;
        mark_object_as_selected(figure);
        figure.show_inside();
        figure.button.highlight_as_selected();
        foreach (Figure_appearance appearance in figure.get_appearances()) {
            highlight(appearance);
        }
    }

    public void activate() {
        enabled = true;
        figure_storage.receiver = this;
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
        observed_figure.hide_inside();
        observed_figure.button.dehighlight_as_selected();
        foreach (Figure_appearance appearance in observed_figure._appearances) {
            dehighlight(appearance);
        }
        
    }
    

    
    public void highlight(Figure_appearance appearance) {
        highlight_generally(appearance);
        appearance.bezier.gameObject.SetActive(true);
        highlight(appearance.appearance_start);
        highlight(appearance.appearance_end);
    }
    public void dehighlight(Figure_appearance appearance) {
        dehighlight_generally(appearance);
        appearance.bezier.gameObject.SetActive(false);
        dehighlight(appearance.appearance_start);
        dehighlight(appearance.appearance_end);
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



    public void on_click(Figure_button figure_button) {
        finish_observing();
        observe(figure_button.figure);
    }

    public void on_click_stencil_interface(Stencil_interface direction) {
    }


}
}