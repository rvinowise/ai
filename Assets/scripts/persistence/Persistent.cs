using System.Collections;
using UnityEngine;

namespace rvinowise.ai.unity.persistence {
public class Persistent : MonoBehaviour {
    public delegate void LoadObjectStateEvent(Persistent_state persistent_state);

    public event LoadObjectStateEvent load_persistent_state;

    public delegate void PrepareToSaveEvent(Persistent_state persistent_state);

    public event PrepareToSaveEvent prepare_to_saving;

    public Persistent_state persistent_state = new Persistent_state();

    
    void Awake() {
        persistent_state.guid = Persistent_state.CreateGuid();
    }

    public void Load(Persistent_state persistent_state) {
        this.persistent_state = persistent_state;
        StartCoroutine(LoadAfterFrame(persistent_state));
    }

    private IEnumerator LoadAfterFrame(Persistent_state persistent_state) {
        // Wait for the next frame so that all objects have been created from ObjectStates
        yield return null;
        load_persistent_state?.Invoke(persistent_state);
    }

    public void PrepareToSave() {
        prepare_to_saving?.Invoke(persistent_state);
    }
}

}