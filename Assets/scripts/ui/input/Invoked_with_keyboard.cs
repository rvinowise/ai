using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
