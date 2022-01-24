
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;

namespace rvinowise.ai.unity {

public class Manual_figure_builder: MonoBehaviour {

    [SerializeField] private Figure_builder builder;
    public Action_history action_history;
    public Figure_storage figure_storage;
    private Figure figure_prefab;
    private Figure built_figure;
    private Figure_representation built_repr;

    public void on_create_empty_figure() {
        built_figure = builder.create_new_figure("f") as Figure;
        figure_storage.append_figure(built_figure);
        built_repr = built_figure.create_representation() as Figure_representation;
        built_repr.show();
    }

}
}