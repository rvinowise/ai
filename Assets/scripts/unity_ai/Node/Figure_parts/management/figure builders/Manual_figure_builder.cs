
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;
using UnityEngine.EventSystems;
using Vector3 = UnityEngine.Vector3;

namespace rvinowise.ai.unity {

public class Manual_figure_builder: MonoBehaviour {

    [SerializeField] private Figure_builder builder;
    private Figure_storage figure_storage => builder.figure_storage;
    private Figure figure_prefab;
    private Figure built_figure;
    private Figure_representation built_repr;
    public bool is_building;

    public void on_create_empty_figure() {
        built_figure = builder.create_new_figure("f") as Figure;
        figure_storage.append_figure(built_figure);
        built_repr = built_figure.create_representation() as Figure_representation;
        Selector.select(built_figure);
        //built_repr.show();
        is_building = true;
    }

    void Update() {

        if (UnityEngine.Input.GetMouseButtonDown(0)) {
            if (
                Unity_input.instance.get_component_under_mouse<Figure_button>() 
                is Figure_button figure_button
            ) {
                Subfigure subfigure = 
                    built_repr.create_subfigure(figure_button.figure) as Subfigure;
                Selector.select(subfigure);
                
            }
        }
    }

    
    
}
}