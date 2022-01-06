using System;
using System.Collections.Generic;
using UnityEngine;

namespace rvinowise.unity.persistence {

[Serializable]
public class Persistent_state {
    public string guid;
    public string objectName;
    public string objectTag;
    public int objectLayer;
    public float[] position;
    public bool isPrefab;
    public string prefabGuid;
    public string[] childrenGuids;
    public Dictionary<string, object> genericValues;

    public Persistent_state() {
        if (string.IsNullOrEmpty(guid)) {
            guid = CreateGuid();
        }
        isPrefab = false;
        prefabGuid = guid;
        genericValues = new Dictionary<string, object>();
    }

    private void PrepareToSave(GameObject gameObject) {
        // If the object state is a prefab, then prefabGuid cannot be empty, and it has to be different from the object's guid
        if (isPrefab) {
            if (string.IsNullOrEmpty(prefabGuid)) {
                throw new InvalidOperationException("Prefab guid is empty even through isPrefab = true");
            }
            if (prefabGuid == guid) {
                throw new InvalidOperationException("Prefab guid == guid");
            }
        }

        objectName = gameObject.name;
        objectTag = gameObject.tag;
        objectLayer = gameObject.layer;
        position = Convertor.ConvertFromVector2(gameObject.transform.position);
        // Let the DynamicObject fill in more information from the game object
        gameObject.GetComponent<Persistent>()?.PrepareToSave();

        List<string> childrenGuidsList = new List<string>();
        foreach (Transform childTransform in gameObject.transform) {
            Persistent dynamicObject = childTransform.GetComponent<Persistent>();
            if (dynamicObject == null) {
                continue;
            }
            // Recursively tell the child to prepare to save
            dynamicObject.persistent_state.PrepareToSave(childTransform.gameObject);
            // Add the child's guid to the list of children
            childrenGuidsList.Add(dynamicObject.persistent_state.guid);
        }
        childrenGuids = childrenGuidsList.ToArray();
    }

    public List<Persistent_state> Save(GameObject gameObject) {
        // Recursively tell gameObject and its descendants to prepare to save
        PrepareToSave(gameObject);
        // The ObjectState of gameObject and its descendants will be saved here
        List<Persistent_state> savedObjects = new List<Persistent_state>();

        // Loop through all the children
        foreach (Transform childTransform in gameObject.transform) {
            Persistent dynamicObject = childTransform.GetComponent<Persistent>();
            if (dynamicObject == null) {
                continue;
            }
            // Save the descendants' object states into a flat list
            savedObjects.AddRange(dynamicObject.persistent_state.Save(childTransform.gameObject));
        }
        // Save this object state into the list as well
        savedObjects.Add(this);

        return savedObjects;
    }

    public static List<Persistent_state> SaveObjects(Transform saved_root) {
        List<Persistent_state> objectStates = new List<Persistent_state>();
        foreach (Transform child in saved_root) {
            Persistent dynamicObject = child.GetComponent<Persistent>();
            if (dynamicObject == null) {
                continue;
            }
            objectStates.AddRange(dynamicObject.persistent_state.Save(child.gameObject));
        }
        return objectStates;
    }

    public static void LoadObjects(
        Dictionary<string, GameObject> prefabs, List<Persistent_state> objectStates, GameObject rootObject
    ) {
        ClearChildren(rootObject);

        Dictionary<string, GameObject> createdObjects = new Dictionary<string, GameObject>();

        foreach (Persistent_state objectState in objectStates) {
            GameObject createdObject;
            Persistent dynamicObject;

            if (objectState.isPrefab) {
                // Do we have a prefab with the required guid?
                if (!prefabs.ContainsKey(objectState.prefabGuid)) {
                    throw new InvalidOperationException("Prefab with guid " + objectState.prefabGuid + " not found.");
                }

                // Instantiate the prefab at the specified position
                createdObject = UnityEngine.Object.Instantiate(prefabs[objectState.prefabGuid]);
                // Find the DynamicObject component and set the object state
                dynamicObject = createdObject.GetComponent<Persistent>();
            }
            else {
                // Create a new object
                createdObject = new GameObject();
                // Add a SaveableObject component and set the object state
                dynamicObject = createdObject.AddComponent<Persistent>();
            }

            dynamicObject.Load(objectState);

            // Find and add the children
            foreach (string childGuid in objectState.childrenGuids) {
                if (!createdObjects.ContainsKey(childGuid)) {
                    Debug.Log("Cannot find child with guid " + childGuid);
                    continue;
                }
                createdObjects[childGuid].transform.SetParent(createdObject.transform);
            }

            // Set the object's name, position, etc, and attach it to the root (it can get attached to a different parent later)
            createdObject.name = objectState.objectName;
            createdObject.tag = objectState.objectTag;
            createdObject.layer = objectState.objectLayer;
            Vector2 position = Convertor.ConvertToVector2(objectState.position);
            createdObject.transform.position = new Vector3(position.x, position.y, 0);
            createdObject.transform.SetParent(rootObject.transform);
            // Save the object into the dictionary
            createdObjects.Add(objectState.guid, createdObject);
        }
    }

    private static void ClearChildren(GameObject root) {
        foreach (Transform child in root.transform) {
            UnityEngine.Object.Destroy(child.gameObject);
        }
    }

    public static string CreateGuid() {
        return Guid.NewGuid().ToString();
    }
}


}