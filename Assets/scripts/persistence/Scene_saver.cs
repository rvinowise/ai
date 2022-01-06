using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using rvinowise.rvi.contracts;
using UnityEngine;


namespace rvinowise.unity.persistence {
public class Scene_saver:
    MonoBehaviour 
{
    
    string SAVE_OBJECTS_PATH;
    [SerializeField]
    Transform saved_object;
    private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings {
        TypeNameHandling = TypeNameHandling.None
    };

    public static Scene_saver instance;

    void Awake() {
        Contract.Assert(instance == null);
        instance = this;
        SAVE_OBJECTS_PATH = Application.dataPath + "/objects.json";
    }
    
    public void on_save_scene() {
        SaveDynamicObjects(SAVE_OBJECTS_PATH);
    }
    

    


    private void SaveDynamicObjects(string path) {
        List<Persistent_state> objectStates = Persistent_state.SaveObjects(saved_object);
        WriteJson(path, objectStates);
        Debug.Log("Saved objects to: " + path);
    }

    private void WriteJson<T>(string path, T obj) {
        string json = JsonConvert.SerializeObject(obj, Formatting.Indented, jsonSerializerSettings);
        File.WriteAllText(path, json);
    }

}
}