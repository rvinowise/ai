using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using rvinowise.rvi.contracts;
using UnityEngine;
using rvinowise.ai.patterns;
using rvinowise.unity.ai;

namespace rvinowise.unity.ai.persistence {
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
        save_actions();
        save_action_groups();
        save_figure_appearances();
        save_subfigures();
        save_patterns();
        save_figures();
    }

    private void save_actions() {
        
        foreach(IAction_group action_group in action_history.get_action_groups(0, action_history.last_moment)) {
            
        }
    }
    
    private void save_action_groups() {

    }
    private void save_figure_appearances() {

    }
    private void save_subfigures() {

    }
    private void save_patterns() {

    }
    private void save_figures() {

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