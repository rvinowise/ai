using System;
using System.Collections.Generic;
using System.Linq;
using abstract_ai;
using rvinowise.ai.patterns;
using rvinowise.rvi.contracts;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.table;
using UnityEngine;

namespace rvinowise.unity.ai {
public class Pattern_storage: MonoBehaviour {
    public Pattern pattern_prefab;
    public List<IPattern> known_patterns = new List<IPattern>();
    public Pattern pleasure_pattern;
    public Pattern pain_pattern;
    public Table pattern_table;
    
    public Dictionary<string, IPattern> name_to_pattern = 
        new Dictionary<string, IPattern>();

    private string[] symbol_patterns = {",",";","=","+","-"};
    void Awake() {
        create_initial_patterns();
    }

    virtual protected void Start() {
        name_to_pattern = create_map_name_to_pattern(
            known_patterns
        );
    }

    private Dictionary<string, IPattern> create_map_name_to_pattern(
        ICollection<IPattern> patterns    
    ) {
        Dictionary<string, IPattern> name_to_pattern = new Dictionary<string, IPattern>();
        foreach (IPattern pattern in patterns) {
            name_to_pattern.Add(pattern.id, pattern);
        }
        return name_to_pattern;
    }

    public IEnumerable<Pattern> get_selected_patterns() {
        IList<Pattern> result = new List<Pattern>();
        foreach(Pattern pattern in known_patterns) {
            if (pattern.selected) {
                result.Add(pattern);
            }
        }
        if (pleasure_pattern.selected) {
            result.Add(pleasure_pattern);
        } else if (pain_pattern.selected) {
            result.Add(pain_pattern);
        }
        
        return result;
    }

    public float get_selected_mood() {
        Contract.Requires(
            (
                (!pleasure_pattern.selected) ||
                (pleasure_pattern.selected != pain_pattern.selected)
                ),
            "either pain or pleasure at the same time"
            );
        if (pleasure_pattern.selected) {
            return 1f;
        } else if (pain_pattern.selected) {
            return -1f;
        }
        return 0f;
    }

    
    private void create_initial_patterns() {
        foreach(string pattern_id in symbol_patterns) {
            Pattern pattern = pattern_prefab.get_for_base_input(
                pattern_id
            );
            Contract.Ensures(pattern != null);
            append_pattern(pattern);
        }
        for (int i=0;i<=9;i++) {
            Pattern pattern = pattern_prefab.get_for_base_input(
                get_id_for_index(i)
            );
            Contract.Ensures(pattern != null);
            append_pattern(pattern);
        }
    }

    private string get_id_for_index(int in_index) {
        return string.Format("{0}",in_index);
    }
    public void append_pattern(IPattern in_pattern) { 
        Pattern pattern = in_pattern as Pattern;

        known_patterns.Add(pattern);
        name_to_pattern.Add(in_pattern.id, in_pattern);
        pattern_table.add_item(pattern);
    }

    public void remove_pattern(IPattern in_pattern) {
        known_patterns.Remove(in_pattern);
        name_to_pattern.Remove(in_pattern.id);
        if (in_pattern is MonoBehaviour unity_pattern) {
            pattern_table.remove_item(unity_pattern);
        }
    }

    public void append_patterns(
        IDictionary<string, IPattern> new_patterns
    ) {
        foreach(var item in new_patterns) {
            if (item.Value is Pattern pattern) {
                append_pattern(pattern);
            }
        }
    }

    public IPattern get_pattern_having(
        IFigure beginning,
        IFigure ending
    ) {
        foreach(var pattern in known_patterns) {
            if (
                pattern.first_half == beginning &&
                pattern.second_half == ending
            ) {
                return pattern;
            }
        }
        return null;
    }

    /* space-separated pattern names  */
    public void select_patterns_from_string(string in_string) {
        deselect_all_patterns();
        string[] names = in_string.Split(' ')
            .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        foreach (string name in names) {
            IPattern pattern;
            name_to_pattern.TryGetValue(name, out pattern);
            if (pattern != null) {
                select_pattern((Pattern)pattern);
            } else {
                Debug.LogErrorFormat(
                    "trying to select non-existing pattern \"{0}\"",name
                );
            }
        }
    }

    public void select_pattern(Pattern in_pattern) {
        in_pattern.selected = true;
    }

    public void toggle_pattern_selection(Pattern in_pattern) {
        in_pattern.selected = !in_pattern.selected;
    }

    public void deselect_all_patterns() {
        foreach (var pattern in get_selected_patterns()) {
            pattern.selected = false;
        }
    }
}
}