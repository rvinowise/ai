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
    public string SAVE_OBJECTS_PATH = Application.dataPath + "/objects.json";

    private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings {
        TypeNameHandling = TypeNameHandling.None
    };

    public static Scene_saver instance;

    void Awake() {
        Contract.Assert(instance == null);
        instance = this;
    }
    
    public void save_scene() {
        SaveDynamicObjects(SAVE_OBJECTS_PATH);
    }
    

    private GameObject GetRootDynamicObject() {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("DynamicRoot")) {
            if (gameObject.activeSelf) {
                return gameObject;
            }
        }
        throw new InvalidOperationException("Cannot find root of dynamic objects");
    }


    private void SaveDynamicObjects(string path) {
        List<Persistent_state> objectStates = Persistent_state.SaveObjects(GetRootDynamicObject());
        WriteJson(path, objectStates);
        Debug.Log("Saved objects to: " + path);
    }

    private void WriteJson<T>(string path, T obj) {
        string json = JsonConvert.SerializeObject(obj, Formatting.Indented, jsonSerializerSettings);
        File.WriteAllText(path, json);
    }

}
}