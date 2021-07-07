using System.Collections;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.patterns;
using rvinowise.rvi.contracts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace rvinowise.unity.ai {

public enum Input_mode {
    One_letter = 1,
    Long = 2,
    Several_short_names = 3,
    Text_field=4
}
public class Human_input : Input {
    public Input_mode input_mode;
    [SerializeField]
    private TMP_InputField input_field;
    
    


    override protected void Awake() {
        base.Awake();
    }
     protected void Start() {

        input_field.Select();
        input_field.ActivateInputField();
    }

    


    private static readonly KeyCode key_submit = KeyCode.Return;
    private void read_input_as_several_one_letter_patterns() {
        if (UnityEngine.Input.GetKeyDown(
            key_submit
        )) {
            receiver.input_selected_patterns();
        }
        foreach (
            KeyValuePair<string, IPattern> item in 
            pattern_storage.name_to_pattern
        ) {
            if (UnityEngine.Input.GetKeyDown(
                item.Key
            )) {
                Contract.Requires(
                    item.Value is Pattern, 
                    "can't input other implementations"
                );
                pattern_storage.toggle_pattern_selection((Pattern)item.Value);
            }
        }
    }



    public void on_enter_clicked() {
        if (entering_control_command()) {
            input_control_commands(input_field.text);
        } else {
            receiver.input_selected_patterns();
            pattern_storage.deselect_all_patterns();
        }

        input_field.text = "";
        input_field.ActivateInputField();
    }

    private void input_control_commands(string commands) {
        if (commands.IndexOf("\\n") >= 0) {
            receiver.start_new_line();
        }
    }

    public void on_text_field_edited() {
        
        if (!entering_control_command()) {
            pattern_storage.select_patterns_from_string(
                input_field.text
            );
        }
    }

    private bool entering_control_command() {
        if (input_field.text.Length == 0) {
            return false;
        }
        return input_field.text[0] == '\\';
    }

    
    
}
}
