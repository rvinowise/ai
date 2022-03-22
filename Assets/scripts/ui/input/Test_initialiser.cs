using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using UnityEngine;
using rvinowise.rvi.contracts;
using rvinowise.ai.unity;
using rvinowise.ai.unity;
using Action = rvinowise.ai.unity.Action;
using rvinowise.ai.general;
using rvinowise.ai.unity;


namespace rvinowise.unity.ui.input {


public class Test_initialiser : MonoBehaviour 
{
    private static Test_initialiser instance;

    public Figure_builder_from_signals figure_builderFromSignals;
    public File_input file_input;
    public Figure_storage figure_storage;
    public Action_history action_history;
    void Awake() {
        Contract.Assert(instance == null, "singleton");
        instance = this;
    }

    void Start() {
        //file_input.read_file("C:\\Users\\rvino\\Desktop/input.txt"); //test example
        // var selected_groups = action_history.get_action_groups(
        //     3,8
        // );
        // IFigure new_figure = figure_builder.create_figure_from_action_history(selected_groups);

    }
    


}
}