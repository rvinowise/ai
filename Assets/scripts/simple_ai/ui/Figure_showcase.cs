using System;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.ai.ui.general;
using rvinowise.ai.ui.unity;
using rvinowise.ai.unity.simple;
using rvinowise.ui.table;
using UnityEngine;

namespace rvinowise.ai.simple {
public class Figure_showcase<TFigure>: 
    IFigure_provider<TFigure>,
    IFigure_button_click_receiver
    where TFigure: class?, IFigure 
{

    
    public IVisual_figure shown_figure;
    public IFigure_button_click_receiver receiver;
    
    public ITable<TFigure> figure_button_table;
    private Figure figure_prefab;
    private Figure_button figure_button_prefab;
    private IFigure_button button_stencil_out;
    private IFigure_button button_stencil_in;


    private IFigure_provider<TFigure> figure_provider;
    private readonly List<IFigure_button> figure_buttons = new List<IFigure_button>();
    private IMode_selector mode_selector;


    public void init() {
        figure_provider = new Figure_provider<TFigure>(create_figure);
        button_stencil_out.click_receiver = this;
        button_stencil_in.click_receiver = this;
    }


    #region IFigure_provider

    public IReadOnlyList<TFigure> get_known_figures() => figure_provider.get_known_figures();
    

    public TFigure provide_figure(string id = "figure") => figure_provider.provide_figure(id);

    public TFigure provide_sequence_for_pair(IFigure beginning_figure, IFigure ending_figure) =>
        figure_provider.provide_sequence_for_pair(
            beginning_figure, ending_figure
        );


    public TFigure find_figure_with_id(string id) =>
        figure_provider.find_figure_with_id(id);

    #endregion IFigure_provider

    private Figure create_figure(string id = "figure") {
        Figure figure = figure_prefab.provide_new<Figure>();
        figure.id = id;
        figure.header.mode_selector = mode_selector;
        figure.transform.parent = figure_folder;
        figure.transform.localPosition = Vector3.zero;
        figure.gameObject.SetActive(false);
        create_button_for_figure(figure);

        return figure;
    }
    private void create_button_for_figure(IVisual_figure figure) {
        Figure_button figure_button = figure_button_prefab.create_for_figure(figure);
        figure.button = figure_button;
        figure_button_table.add_item(figure_button);
        figure_buttons.Add(figure_button);
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
        IFigure_button button = figure_buttons.First(button => button.figure == figure);
        figure_button_table.remove_item(button);
        figure_buttons.Remove(button);
    }
    
    
    public void show_insides_of_one_figure(IVisual_figure new_shown_figure) {
        new_shown_figure.show();
        shown_figure.hide();
    }

    #region IFigure_button_click_receiver
    public void on_click(IFigure_button figure_button) {
    }

    public void on_click_stencil_interface(Stencil_interface direction) {
    }
    
    #endregion
}
}