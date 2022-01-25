using System.Numerics;
using rvinowise.ai.general;
using System.Collections.Generic;
using System;
using rvinowise.unity;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.extensions.pooling;
using rvinowise.unity.ui.input;
using rvinowise.unity.ui.input.mouse;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;


namespace rvinowise.ai.unity {

public class Figure_button:
    MonoBehaviour 
{
    public TMP_Text lable;

    public Figure figure;

    [SerializeField] private Image selectable_image;
    
    [called_by_prefab]
    public Figure_button create_for_figure(Figure figure) {
        Figure_button figure_button = this.provide_new<Figure_button>();
        figure_button.lable.text = figure.id;
        figure_button.figure = figure;
        return figure_button;

    }
    //inspector event
    public void on_click() {
        toggle_showing_figure();
    }

    private void toggle_showing_figure() {
        if (Selector.instance.selected(figure)) {
            Selector.deselect(figure);
        } else {
            Selector.select(figure);
        }
    }
    

    public void highlight_as_selected() {
        selectable_image.color = Selector.instance.selected_color;
    }
    public void dehighlight_as_selected() {
        selectable_image.color = Selector.instance.normal_color;
    }
}
}