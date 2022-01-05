using System.Collections;
using UnityEngine;

namespace rvinowise.unity.persistence {
public class Persistent : MonoBehaviour {
    public delegate void LoadObjectStateEvent(Persistent_state persistent_state);

    public event LoadObjectStateEvent loadObjectStateDelegates;

    public delegate void PrepareToSaveEvent(Persistent_state persistent_state);

    public event PrepareToSaveEvent prepareToSaveDelegates;

    public Persistent_state persistentState;

    void Start() {
        if ((persistentState != null) && persistentState.isPrefab && persistentState.guid.Equals(persistentState.prefabGuid)) {
            // Create a unique guid for each prefab instantiation
            persistentState.guid = Persistent_state.CreateGuid();
        }
    }

    public void Load(Persistent_state persistent_state) {
        this.persistentState = persistent_state;
        StartCoroutine(LoadAfterFrame(persistent_state));
    }

    private IEnumerator LoadAfterFrame(Persistent_state persistent_state) {
        // Wait for the next frame so that all objects have been created from ObjectStates
        yield return null;
        loadObjectStateDelegates?.Invoke(persistent_state);
    }

    public void PrepareToSave() {
        prepareToSaveDelegates?.Invoke(persistentState);
    }
}

}