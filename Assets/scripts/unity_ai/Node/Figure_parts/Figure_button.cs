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
using Vector3 = UnityEngine.Vector3;


namespace rvinowise.ai.unity {

public class Figure_button:
    MonoBehaviour 
{
    public TMP_Text lable;

    public Figure figure;

    public bool is_active;
    
    [called_by_prefab]
    public Figure_button create_for_figure(IFigure figure) {
        Figure_button figure_button = this.provide_new<Figure_button>();
        figure_button.lable.text = figure.id;
        return figure_button;

    }
    //inspector event
    public void on_click() {
        toggle_showing_figure();
    }

    private void toggle_showing_figure() {
        if (is_active) {
            hide_figure();
        }
        else {
            show_figure();
        }
    }

    private void show_figure() {
        is_active = true; 
        figure.gameObject.SetActive(true);
    }
    
    private void hide_figure() {
        is_active = false; 
        figure.gameObject.SetActive(false);
    }
}
}