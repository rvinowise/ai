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
public class Figure_storage: MonoBehaviour {
    public List<IFigure> known_figures = new List<IFigure>();
    public List<IFigure> known_sequential_figures = new List<IFigure>();
    public Table figure_button_table;
    public Figure pleasure_signal;
    public Figure pain_signal;
    [SerializeField] private Transform figure_folder;
    
    public Dictionary<string, IFigure> name_to_figure = 
        new Dictionary<string, IFigure>();
    
    

    public Figure figure_prefab;
    public Figure_button figure_button_prefab;
    

    void Awake() {
        figure_button_table.init(figure_button_prefab);
    }

    
    

    public void append_figure(IFigure figure) { 

        known_figures.Add(figure);
        name_to_figure.Add(figure.id, figure);

        if (figure is Figure unity_figure) {
            unity_figure.transform.parent = figure_folder;
            unity_figure.transform.localPosition = Vector3.zero;
            unity_figure.gameObject.SetActive(false);
            create_button_for_figure(unity_figure);
        }
        
    }

    private void create_button_for_figure(Figure figure) {
        Figure_button figure_button = figure_button_prefab.create_for_figure(figure);
        figure.button = figure_button;
        figure_button_table.add_item(figure_button);
    }

    


    public void remove_figure(IFigure figure) {
        known_figures.Remove(figure);
        name_to_figure.Remove(figure.id);
        if (figure is MonoBehaviour unity_figure) {
            figure_button_table.remove_item(unity_figure);
        }
    }
    
    
    public IFigure find_figure_with_id(string id) {
        IFigure figure;
        name_to_figure.TryGetValue(id, out figure);
        return figure;
    }
}
}