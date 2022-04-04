using System;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.ai.simple;
using rvinowise.rvi.contracts;
using rvinowise.ai.unity;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;
using rvinowise.unity.ui.table;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace rvinowise.ai.unity {
public class Figure_showcase: 
    MonoBehaviour,
    IFigure_provider 
{

    private IFigure_provider figure_provider;
    
    private readonly List<Figure_button> figure_buttons = new List<Figure_button>();
    public Figure shown_figure;
    public Table figure_button_table;
    [SerializeField] private Figure figure_prefab;
    [SerializeField] private Figure_button figure_button_prefab;
    [SerializeField] private Transform figure_folder;
    [SerializeField] private Figure_button button_stencil_out;
    [SerializeField] private Figure_button button_stencil_in;
    [SerializeField] private Mode_selector mode_selector;
    
    public IFigure_button_click_receiver receiver;

    public void init() {
        figure_provider = new Figure_provider(create_figure);
        figure_button_table.init(figure_button_prefab);
        button_stencil_out.showcase = this;
        button_stencil_in.showcase = this;
    }


    #region IFigure_provider

    public IReadOnlyList<IFigure> get_known_figures() => figure_provider.get_known_figures();

    public IFigure create_next_figure(string prefix = "") {
        Figure figure = figure_provider.create_next_figure(prefix) as Figure;
        
        return figure;
    }

    public IFigure create_base_signal(string id = "") {
        Figure figure = figure_provider.create_base_signal(id)  as Figure;
            
        return figure;
    }

    public IFigure provide_sequence_for_pair(IFigure beginning_figure, IFigure ending_figure) {
        Figure figure = figure_provider.provide_sequence_for_pair(
            beginning_figure,ending_figure
        ) as Figure;
            
        return figure;
    }


    public IFigure find_figure_with_id(string id) =>
        figure_provider.find_figure_with_id(id);
    

    #endregion IFigure_provider
    
    private Figure create_figure(string id = "figure") {
        Figure figure = figure_prefab.provide_new<Figure>();
        figure.id = id;
        figure.header.mode_selector = mode_selector;
        append_figure_visuals(figure);

        return figure;
    }

    private void append_figure_visuals(unity.Figure figure) {
        figure.transform.parent = figure_folder;
        figure.transform.localPosition = Vector3.zero;
        figure.gameObject.SetActive(false);
        create_button_for_figure(figure);
    }
    
    public void remove_figure(IFigure figure) {
        figure_provider.remove_figure(figure);
        if (figure is Figure unity_figure) {
            remove_button_for_figure(unity_figure);
        }
    }

    private void create_button_for_figure(Figure figure) {
        Figure_button figure_button = figure_button_prefab.create_for_figure(figure);
        figure.button = figure_button;
        figure_button.showcase = this;
        figure_button_table.add_item(figure_button);
        figure_buttons.Add(figure_button);
    }


    private void remove_button_for_figure(Figure figure) {
        Figure_button button = figure_buttons.First(button => button.figure == figure);
        figure_button_table.remove_item(button);
        figure_buttons.Remove(button);
    }
    
    
    public void show_insides_of_one_figure(Figure new_shown_figure) {
        new_shown_figure.show_inside();
        shown_figure.hide_inside();
    }


}
}