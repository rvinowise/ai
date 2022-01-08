using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using rvinowise.rvi.contracts;
using UnityEngine;


namespace rvinowise.ai.unity.persistence {
public class Network_persistence: 
    MonoBehaviour
{
    private JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings
        {TypeNameHandling = TypeNameHandling.None};

    public static Network_persistence instance;
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