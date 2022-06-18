using System.Collections;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.unity.ui.input;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace rvinowise.ai.unity {

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

    #region network modules

    ISequence_finder<Figure> sequence_finder => network.sequence_finder;
    
    #endregion network modules


    protected void Start() {

        input_field.Select();
        input_field.ActivateInputField();
    }

    public void on_enter_clicked() {
        if (entering_control_command()) {
            input_control_commands(input_field.text);
        } else {
            receiver.input_selected_signals();
            Selector.instance.deselect_all_figures();
        }

        input_field.text = "";
        input_field.ActivateInputField();
    }

    public void on_text_field_edited() {
        if (!entering_control_command()) {
            Selector.instance.select_figures_from_string(
                input_field.text
            );
        }
    }

    public void on_find_sequences_clicked() {
        sequence_finder.enrich_storage_with_sequences();
    }
    
    private void input_control_commands(string commands) {
        if (commands.IndexOf("\\n") >= 0) {
            receiver.start_new_line();
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
