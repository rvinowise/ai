using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using rvinowise.rvi.contracts;
using UnityEngine;


namespace rvinowise.unity.ai.persistence {
public class Network_loader: 
    MonoBehaviour
{

    // The directory under Resources that the dynamic objects' prefabs can be loaded from
    private string PREFABS_PATH = "Prefabs/";
    // A dictionary of prefab guid to prefab
    public Dictionary<string, GameObject> prefabs;

    public string SAVE_OBJECTS_PATH;

    private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {TypeNameHandling = TypeNameHandling.None};
    
    public static Network_loader instance;
    void Awake() {
        Contract.Assert(instance == null);
        instance = this;
        SAVE_OBJECTS_PATH = Application.dataPath + "/objects.json";
        prefabs = LoadPrefabs(PREFABS_PATH);
    }
    
    
    private Dictionary<string, GameObject> LoadPrefabs(string prefabsPath) {
        Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

        GameObject[] allPrefabs = Resources.LoadAll<GameObject>(prefabsPath);
        foreach (GameObject prefab in allPrefabs) {
            Persistent dynamicObject = prefab.GetComponent<Persistent>();
            if (dynamicObject == null) {
                throw new InvalidOperationException("Prefab does not contain DynamicObject");
            }
            if (!dynamicObject.persistent_state.isPrefab) {
                throw new InvalidOperationException("Prefab's ObjectState isPrefab = false");
            }
            prefabs.Add(dynamicObject.persistent_state.prefabGuid, prefab);
        }

        Debug.Log("Loaded " + prefabs.Count + " saveable prefabs.");
        return prefabs;
    }

    public void on_load_scene() {
        LoadDynamicObjects(SAVE_OBJECTS_PATH);
    }

    public Persistent FindDynamicObjectByGuid(string guid) {
        Persistent[] dynamicObjects = GetRootDynamicObject().GetComponentsInChildren<Persistent>();
        foreach (Persistent dynamicObject in dynamicObjects) {
            if (dynamicObject.persistent_state.guid.Equals(guid)) {
                return dynamicObject;
            }
        }
        return null;
    }

    private GameObject GetRootDynamicObject() {
        foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("DynamicRoot")) {
            if (gameObject.activeSelf) {
                return gameObject;
            }
        }
        throw new InvalidOperationException("Cannot find root of dynamic objects");
    }



    private void LoadDynamicObjects(string path) {
        List<Persistent_state> objectStates = ReadJson<List<Persistent_state>>(path);
        Persistent_state.LoadObjects(prefabs, objectStates, GetRootDynamicObject());
        Debug.Log("Loaded objects from: " + path);
    }


    private T ReadJson<T>(string path) {
        string json = File.ReadAllText(path);
        return JsonConvert.DeserializeObject<T>(json, jsonSerializerSettings);
    }

  
}
}