using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using rvinowise.rvi.contracts;
using UnityEngine;
using rvinowise.ai.general;
using rvinowise.ai.unity;
using rvinowise.ai.unity.persistence.serializable;

namespace rvinowise.ai.unity.persistence {
public class Network_saver:
    MonoBehaviour 
{
    
    string SAVE_OBJECTS_PATH;
    [SerializeField]
    Transform saved_object;
    [SerializeField] IAction_history action_history;
    [SerializeField] Pattern_storage pattern_storage;
    [SerializeField] Figure_storage figure_storage;
    private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings {
        TypeNameHandling = TypeNameHandling.None
    };

    public static Network_saver instance;

    void Awake() {
        Contract.Assert(instance == null);
        instance = this;
        SAVE_OBJECTS_PATH = Application.dataPath + "/objects.json";
    }
    
    public void on_save() {
        create_serializable_network();
        
    }

    public void create_serializable_network() {
        serializable.Network out_network = new serializable.Network();
        serialize_action_groups(out_network);
        serialize_patterns(out_network);
        serialize_figures(out_network);
    }

    private void serialize_action_groups(serializable.Network out_network) {
        foreach(
            IAction_group action_group in 
            action_history.get_action_groups(
                0, action_history.last_moment
            )
        ) {
            out_network.action_groups.Add(
                new serializable.Action_group(action_group)
            );
        }
    }
    
    
    private void serialize_patterns(serializable.Network out_network) {
        foreach(
            IPattern pattern in 
            pattern_storage.known_patterns
        ) {
            out_network.patterns.Add(
                new serializable.Pattern(pattern)
            );
            serialize_figure_appearances(out_network, pattern);
        }
    }
    private void serialize_figures(serializable.Network out_network) {
        foreach(
            Figure figure in 
            figure_storage.known_figures
        ) {
            out_network.figures.Add(
                new serializable.Figure(figure)
            );
            serialize_subfigures(out_network, figure);
            serialize_figure_appearances(out_network, figure);
        }
    }
    private void serialize_subfigures(
        serializable.Network out_network,
        unity.Figure figure
    ) {
        foreach(
            ISubfigure subfigure in 
            figure.subfigures
        ) {
            out_network.subfigures.Add(
                new serializable.Subfigure(subfigure)
            );
        }
    }

    private void serialize_figure_appearances(
        serializable.Network out_network,
        general.IFigure figure
    ) {
        foreach(
            IFigure_appearance appearance in 
            figure.get_appearances_in_interval(
                0,
                action_history.end_moment
            )
        ) {
            out_network.figure_appearances.Add(
                new serializable.Figure_appearance(appearance)
            );
        }
    }

    private void SaveDynamicObjects(string path) {
        List<Persistent_state> objectStates = Persistent_state.SaveObjects(saved_object);
        WriteJson(path, objectStates);
    }

    private void WriteJson<T>(string path, T obj) {
        string json = JsonConvert.SerializeObject(obj, Formatting.Indented, jsonSerializerSettings);
        File.WriteAllText(path, json);
    }

}
}