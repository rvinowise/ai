﻿
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;

namespace rvinowise.ai.unity {

public class Figure_provider:
    IFigure_provider
{

    private Figure_storage figure_storage;
    private Figure figure_prefab;
        

    void Awake() {
        figure_prefab = figure_storage.figure_prefab;
    }
    
  
    private Dictionary<string,int> last_ids = new Dictionary<string, int>();
    public IFigure create_new_figure(string prefix = "") {
        Figure new_figure = figure_prefab.provide_new<Figure>();
        new_figure.id = get_next_id_for_prefix(prefix);
        new_figure.header.mode_selector = mode_selector;
        return new_figure;
    }

    private string get_next_id_for_prefix(string prefix) {
        int next_id = 0;
        last_ids.TryGetValue(prefix, out next_id);
        last_ids[prefix] = ++next_id;
        return $"{prefix}{next_id}";
    }



  


}
}