using System.Collections.Generic;
using rvinowise.ai.general;
using rvinowise.ai.simple;
using rvinowise.ai.ui.general;
using rvinowise.ai.ui.unity;
using rvinowise.ai.unity.simple;
using rvinowise.unity.extensions;
using UnityEngine;


namespace rvinowise.ai.unity {
public class Figure_showcase: 
    MonoBehaviour,
    IFigure_showcase<Figure>,
    IFigure_button_click_receiver
{
    
    public IVisual_figure shown_figure { get; private set; }
    
    #region unity inspector
    public Button_table button_table;
    [SerializeField] public Figure figure_prefab;
    [SerializeField] private Transform shown_figure_folder;

    #endregion unity inspector

    private IFigure_provider<Figure> figure_provider;


    public void Awake() {
        figure_provider = new Figure_provider<Figure>(create_figure);
        button_table.higher_click_receiver = this;

    }


    #region IFigure_showcase

    public void show_insides_of_one_figure(IVisual_figure new_shown_figure) {
        new_shown_figure.show();
        shown_figure?.hide();
        shown_figure = new_shown_figure;
    }

    public IFigure_button get_button_for_figure(IVisual_figure figure) =>
        button_table.get_button_for_figure(figure);
    
    #endregion
    
    #region IFigure_provider

    public IReadOnlyList<Figure> get_known_figures() => figure_provider.get_known_figures();
    

    public Figure provide_figure(string id = "figure") => figure_provider.provide_figure(id);

    public Figure provide_sequence_for_pair(IFigure beginning_figure, IFigure ending_figure) =>
        figure_provider.provide_sequence_for_pair(
            beginning_figure, ending_figure
        );


    public Figure find_figure_with_id(string id) =>
        figure_provider.find_figure_with_id(id);

    #endregion IFigure_provider

    //unity
    private Figure create_figure(string id = "figure") {
        Figure figure = figure_prefab.provide_new<Figure>();
        figure.id = id;
        figure.transform.parent = shown_figure_folder;
        figure.transform.localPosition = Vector3.zero;
        figure.hide();
        button_table.create_button_for_figure(figure);

        return figure;
    }
    

    public string get_next_id_for_prefix(string prefix) => 
        figure_provider.get_next_id_for_prefix(prefix);


    public void remove_figure(IFigure figure) {
        figure_provider.remove_figure(figure);
        if (figure is Figure unity_figure) {
            button_table.remove_button_for_figure(unity_figure);
        }
    }

    
 
    #region IFigure_button_click_receiver
    public void on_click(IFigure_button figure_button) {
        show_insides_of_one_figure(figure_button.figure);
    }

    public void on_click_stencil_interface(Stencil_interface direction) {
    }
    
    #endregion

    
}
}