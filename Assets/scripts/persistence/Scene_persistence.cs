using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using rvinowise.rvi.contracts;
using UnityEngine;


namespace rvinowise.unity.persistence {
public class Scene_persistence: 
    MonoBehaviour
{

    // The directory under Resources that the dynamic objects' prefabs can be loaded from
    private string PREFABS_PATH = "Prefabs/";
    // A dictionary of prefab guid to prefab
    public Dictionary<string, GameObject> prefabs;

    public string SAVE_OBJECTS_PATH = Application.dataPath + "/objects.json";

    private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {TypeNameHandling = TypeNameHandling.None};

    public static Scene_persistence instance;
    void Awake() {
        Contract.Assert(instance == null);
        instance = this;
    }
    
    private GameObject GetRootDynamicObject() {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("DynamicRoot")) {
            if (gameObject.activeSelf) {
                return gameObject;
            }
        }
        throw new InvalidOperationException("Cannot find root of dynamic objects");
    }



  
}
}