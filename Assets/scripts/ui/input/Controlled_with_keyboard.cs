using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Controlled_with_keyboard : MonoBehaviour {
    
    [SerializeField] private KeyCode key;
    
    private Button button;

    void Awake() {
        button = GetComponent<Button>();
    }
    
    void Update()
    {
        if (Input.GetKeyDown(key)) {
            button.onClick.Invoke();
        }
    }
}
