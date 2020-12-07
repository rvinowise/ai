using System.Collections;
using System.Collections.Generic;
using System.Linq;
using rvinowise.rvi.contracts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace rvinowise.unity.ai.patterns {

public enum Input_mode {
    One_letter = 1,
    Long = 2,
    Several_short_names = 3,
    Text_field=4
}
public class Human_input : patterns.Input {
    public Input_receiver receiver;
    public Pattern_storage pattern_storage;
    public Input_mode input_mode;
    [SerializeField]
    private TMP_InputField input_field;
    
    private Dictionary<string, Pattern> name_to_pattern = 
        new Dictionary<string, Pattern>();
    private ISet<Pattern> selected_patterns = new HashSet<Pattern>();


    void Awake() {
        pattern_storage = receiver.pattern_storage;
    }
    void Start() {
        name_to_pattern = create_map_name_to_pattern(
            pattern_storage.known_patterns
        );

        input_field.Select();
        input_field.ActivateInputField();
    }

    private Dictionary<string, Pattern> create_map_name_to_pattern(
        ICollection<Pattern> patterns    
    ) {
        Dictionary<string, Pattern> name_to_pattern = new Dictionary<string, Pattern>();
        foreach (Pattern pattern in patterns) {
            name_to_pattern.Add(pattern.id, pattern);
        }
        return name_to_pattern;
    }


    private static readonly KeyCode key_submit = KeyCode.Return;
    private void read_input_as_several_one_letter_patterns() {
        if (UnityEngine.Input.GetKeyDown(
            key_submit
        )) {
            input_selected_patterns();
        }
        foreach (KeyValuePair<string, Pattern> item in name_to_pattern) {
            if (UnityEngine.Input.GetKeyDown(
                item.Key
            )) {
                toggle_pattern_selection(item.Value);
            }
        }
    }


    private void input_selected_patterns() {
        receiver.input_selected_patterns();

        deselect_all_patterns();
    }


    public void on_enter_clicked() {
        if (selected_patterns.Any()) {
            input_selected_patterns();
        }
        input_field.text = "";
        input_field.ActivateInputField();
    }

    public void on_text_field_edited() {
        select_patterns_from_string(
            input_field.text
        );
    }

    private void select_patterns_from_string(string in_string) {
        deselect_all_patterns();
        string[] names = in_string.Split(' ')
            .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        foreach (string name in names) {
            Pattern pattern;
            name_to_pattern.TryGetValue(name, out pattern);
            if (pattern) {
                select_pattern(pattern);
            }
        }
    }

    private void select_pattern(Pattern in_pattern) {
        in_pattern.selected = true;
        selected_patterns.Add(in_pattern);
    }

    private void toggle_pattern_selection(Pattern in_pattern) {
        if (in_pattern.selected) {
            selected_patterns.Remove(in_pattern);
        }
        else {
            selected_patterns.Add(in_pattern);
        }
        in_pattern.selected = !in_pattern.selected;
    }

    private void deselect_all_patterns() {
        foreach (var pattern in selected_patterns) {
            pattern.selected = false;
        }
        selected_patterns.Clear();
        
    }
    
}
}
