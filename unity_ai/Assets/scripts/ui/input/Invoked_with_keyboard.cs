using UnityEngine;
using UnityEngine.Events;


public class Invoked_with_keyboard : MonoBehaviour {
    
    [SerializeField] private KeyCode key;

    public UnityEvent invokable;

    void Awake() {
    }
    
    void Update()
    {
        if (Input.GetKeyDown(key)) {
            invokable.Invoke();
        }
    }
}
