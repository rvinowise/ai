using rvinowise.ai.ui.general;
using rvinowise.ai.unity.simple;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.ui.input;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace rvinowise.ai.ui.unity {


public class Figure_button:
    MonoBehaviour,
    IFigure_button
{
    public TMP_Text label;

    public Stencil_interface stencil_interface;

    [SerializeField] private Image selectable_image;

    public IFigure_button_click_receiver click_receiver { get; set; }

    public IVisual_figure figure { get; private set; }

    [called_by_prefab]
    public Figure_button create_for_figure(IVisual_figure figure) {
        Figure_button figure_button = this.provide_new<Figure_button>();
        figure_button.label.text = figure.id;
        figure_button.figure = figure;
        return figure_button;

    }

    IFigure_button IFigure_button.create_for_figure(IVisual_figure figure) =>
        create_for_figure(figure);



    [called_by_prefab]
    public IFigure_button create_for_stencil_node(Stencil_interface direciton) {
        Figure_button figure_button = this.provide_new<Figure_button>();
        figure_button.label.text = direciton == Stencil_interface.input ? "i" : "o";
        return figure_button;

    }
    //inspector event
    public void on_click() {
        click_receiver.on_click(this);
    }

    #region IAccept_selection

    public Selection_of_object selection { get; }

    #endregion IAccept_selection
}
}