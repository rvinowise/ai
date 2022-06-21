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
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace rvinowise.ai.unity {
public class Button_table: 
    MonoBehaviour,
    IButton_table<Figure_button>, 
    IFigure_button_click_receiver
{
    
    public IVisual_figure shown_figure { get; private set; }
    
    #region unity inspector
    public Table figure_button_table;
    [SerializeField] private Figure figure_prefab;
    [SerializeField] private Figure_button figure_button_prefab;
    [SerializeField] private Transform shown_figure_folder;
    [SerializeField] private Figure_button button_stencil_out;
    [SerializeField] private Figure_button button_stencil_in;

    #endregion unity inspector

    private IFigure_provider<Figure> figure_provider;
    private readonly IDictionary<IVisual_figure, IFigure_button> figure_buttons = 
        new Dictionary<IVisual_figure, IFigure_button>();
    private IMode_selector mode_selector;


    public void init() {
        button_stencil_out.click_receiver = this;
        button_stencil_in.click_receiver = this;
    }




    
    private void create_button_for_figure(IVisual_figure figure) {
        Figure_button figure_button = figure_button_prefab.create_for_figure(figure);
        figure.button = figure_button;
        figure_button.click_receiver = this;
        figure_button_table.add_item(figure_button);
        figure_buttons.Add(figure, figure_button);
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
    

    #region IButton_table
    
    public IFigure_button_click_receiver click_receiver { get; private set; }
    
    public IFigure_button get_button_for_figure(IVisual_figure figure) {
        figure_buttons.TryGetValue(figure, out var out_button);
        return out_button;
    }
    
    #endregion IButton_table
}
}