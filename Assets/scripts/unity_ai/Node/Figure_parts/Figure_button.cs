using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.ui.input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace rvinowise.ai.unity {


public interface IFigure_button_click_receiver {
    void on_click(Figure_button figure_button);
    void on_click_stencil_interface(Stencil_interface direction);
}
public class Figure_button:
    MonoBehaviour 
{
    public TMP_Text lable;

    public Figure figure;
    public Stencil_interface stencil_interface;

    [SerializeField] private Image selectable_image;

    public Figure_storage storage;
    public IFigure_button_click_receiver receiver => storage.receiver;
    
    [called_by_prefab]
    public Figure_button create_for_figure(Figure figure) {
        Figure_button figure_button = this.provide_new<Figure_button>();
        figure_button.lable.text = figure.id;
        figure_button.figure = figure;
        return figure_button;

    }
    
    [called_by_prefab]
    public Figure_button create_for_stencil_node(string type) {
        Figure_button figure_button = this.provide_new<Figure_button>();
        figure_button.lable.text = type;
        return figure_button;

    }
    //inspector event
    public void on_click() {
        if (figure) {
            receiver.on_click(this);
        }
        else {
            receiver.on_click_stencil_interface(stencil_interface);
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