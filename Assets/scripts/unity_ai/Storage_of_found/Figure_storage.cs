using System;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.ai.unity;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;
using rvinowise.unity.ui.table;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UI;

namespace rvinowise.ai.unity {
public class Figure_storage: 
    MonoBehaviour,
    IFigure_storage {
    
    private readonly List<IFigure> known_figures = new List<IFigure>();
    public readonly List<Figure_button> figure_buttons = new List<Figure_button>();
    public Figure shown_figure;
    public Table figure_button_table;
    [SerializeField] private unity.Figure figure_prefab;
    [SerializeField] private Figure_button figure_button_prefab;
    [SerializeField] private Transform figure_folder;
    [SerializeField] private Figure_button button_stencil_out;
    [SerializeField] private Figure_button button_stencil_in;

    public IFigure_button_click_receiver receiver;
    
    public Dictionary<string, IFigure> name_to_figure = 
        new Dictionary<string, IFigure>();
    
    

    void Awake() {
        figure_button_table.init(figure_button_prefab);
        button_stencil_out.storage = this;
        button_stencil_in.storage = this;
    }

    public IReadOnlyList<IFigure> get_known_figures() => known_figures.AsReadOnly();

    public IFigure provide_new_figure(string id) {
        Figure figure = figure_prefab.provide_new<Figure>();
        figure.id = id;
        append_figure(figure);
        return figure;
    }
    public void append_figure(unity.Figure figure) { 
        known_figures.Add(figure);
        name_to_figure.Add(figure.id, figure);

        figure.transform.parent = figure_folder;
        figure.transform.localPosition = Vector3.zero;
        figure.gameObject.SetActive(false);
        create_button_for_figure(figure);
        
    }
    
    public void remove_figure(IFigure figure) {
        known_figures.Remove(figure);
        name_to_figure.Remove(figure.id);
        if (figure is Figure unity_figure) {
            remove_button_for_figure(unity_figure);
            
        }
    }

    private void create_button_for_figure(Figure figure) {
        Figure_button figure_button = figure_button_prefab.create_for_figure(figure);
        figure.button = figure_button;
        figure_button.storage = this;
        figure_button_table.add_item(figure_button);
        figure_buttons.Add(figure_button);
    }



    

    private void remove_button_for_figure(Figure figure) {
        Figure_button button = figure_buttons.First(button => button.figure == figure);
        figure_button_table.remove_item(button);
        figure_buttons.Remove(button);
    }
    
    
    public IFigure find_figure_with_id(string id) {
        IFigure figure;
        name_to_figure.TryGetValue(id, out figure);
        return figure;
    }

    public void show_insides_of_one_figure(Figure shown_figure) {
        shown_figure.show_inside();
        foreach (Figure figure in known_figures) {
            if (shown_figure != figure) {
                figure.hide_inside();
            }
        }
    }
}
}