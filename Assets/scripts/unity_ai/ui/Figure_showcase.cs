using System;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.ai.simple;
using rvinowise.ai.ui.general;
using rvinowise.ai.ui.unity;
using rvinowise.rvi.contracts;
using rvinowise.ai.unity;
using rvinowise.ai.unity.simple;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;
using rvinowise.unity.ui.table;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace rvinowise.ai.unity {
public class Figure_showcase: 
    MonoBehaviour,
    IFigure_showcase,
    IFigure_provider<Figure>,
    IFigure_button_click_receiver
{

    
    public IVisual_figure shown_figure { get; private set; }
    public IFigure_button_click_receiver receiver;
    
    #region unity inspector
    public Table figure_button_table;
    //unity
    [SerializeField] private Figure figure_prefab;
    [SerializeField] private Figure_button figure_button_prefab;
    [SerializeField] private Transform figure_folder;
    [SerializeField] private Figure_button button_stencil_out;
    [SerializeField] private Figure_button button_stencil_in;

    #endregion unity inspector

    private IFigure_provider<Figure> figure_provider;
    private readonly IDictionary<IVisual_figure, IFigure_button> figure_buttons = 
        new Dictionary<IVisual_figure, IFigure_button>();
    private IMode_selector mode_selector;


    public void init() {
        figure_provider = new Figure_provider<Figure>(create_figure);
        button_stencil_out.click_receiver = this;
        button_stencil_in.click_receiver = this;
    }


    #region IFigure_showcase


    public void show_insides_of_one_figure(IVisual_figure new_shown_figure) {
        new_shown_figure.show();
        shown_figure.hide();
    }

    public IFigure_button get_button_for_figure(IVisual_figure figure) {
        figure_buttons.TryGetValue(figure, out var out_button);
        return out_button;
    }
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
        figure.transform.parent = figure_folder;
        figure.transform.localPosition = Vector3.zero;
        figure.gameObject.SetActive(false);
        create_button_for_figure(figure);

        return figure;
    }
    
    //unity
    private void create_button_for_figure(IVisual_figure figure) {
        Figure_button figure_button = figure_button_prefab.create_for_figure(figure);
        figure.button = figure_button;
        figure_button.click_receiver = this;
        figure_button_table.add_item(figure_button);
        figure_buttons.Add(figure, figure_button);
    }
    public string get_next_id_for_prefix(string prefix) => 
        figure_provider.get_next_id_for_prefix(prefix);


    public void remove_figure(IFigure figure) {
        figure_provider.remove_figure(figure);
        if (figure is Figure unity_figure) {
            remove_button_for_figure(unity_figure);
        }
    }

    


    private void remove_button_for_figure(Figure figure) {
        var button_key = figure_buttons.First(
            button => button.Value.figure == figure
        );
        figure_button_table.remove_item(button_key.Value as MonoBehaviour);
        figure_buttons.Remove(button_key);
    }
    
 
    #region IFigure_button_click_receiver
    public void on_click(IFigure_button figure_button) {
    }

    public void on_click_stencil_interface(Stencil_interface direction) {
    }
    
    #endregion

    
}
}