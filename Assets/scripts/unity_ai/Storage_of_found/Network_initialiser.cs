using System;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.ai.unity;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.table;
using UnityEngine;
using rvinowise.unity.ui.input;

namespace rvinowise.ai.unity {
public class Network_initialiser: MonoBehaviour {
    public Figure_storage figure_storage;
    [SerializeField] private Mode_selector mode_selector; 
    
    private readonly string[] symbol_figures = {",",";","=","+","-"};

    void Start() {
        create_base_signals();
    }
    
    private void create_base_signals() {
        foreach(string pattern_id in symbol_figures) {
            IFigure figure = create_base_signal(
                pattern_id
            );
            Contract.Ensures(figure != null);
            figure_storage.append_figure(figure);
        }
        for (int i=0;i<=9;i++) {
            IFigure figure = create_base_signal(
                get_id_for_index(i)
            );
            Contract.Ensures(figure != null);
            figure_storage.append_figure(figure);
        }
    }

    private IFigure create_base_signal(string id) {
        Figure signal = figure_storage.figure_prefab.provide_new<Figure>();
        signal.id = id;
        signal.name = string.Format("signal {0}", signal.id);
        signal.header.mode_selector = mode_selector;
        return signal;
    }
    
    private string get_id_for_index(int in_index) {
        return $"{in_index}";
    }
    
}
}