using System;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.patterns;
using rvinowise.unity.extensions;
using UnityEngine;

namespace rvinowise.unity.ai {
public class Pattern_storage: MonoBehaviour {
    public Pattern pattern_prefab;
    public List<IPattern> known_patterns = new List<IPattern>();
    public Canvas pattern_table;
    
    void Awake() {
        init_test_patterns(5);
    }

    public IEnumerable<Pattern> get_selected_patterns() {
        IList<Pattern> result = new List<Pattern>();
        foreach(Pattern pattern in known_patterns) {
            if (pattern.selected) {
                result.Add(pattern);
            }
        }
        return result;
    }
    private void init_test_patterns(int qty) {
        for (int i=0;i<qty;i++) {
            Pattern pattern = pattern_prefab.get_for_base_input(
                get_id_for_index(i)
            );
            append_known_pattern(pattern);
        }
    }

    private string get_id_for_index(int in_index) {
        return string.Format("{0}",in_index);
    }
    private void append_known_pattern(Pattern in_pattern) { 
        known_patterns.Add(in_pattern);
        
        pattern_table.add_item(in_pattern);
    }

    public void append_patterns(
        IDictionary<string, IPattern> new_patterns
    ) {
        /* known_patterns.Union(
            new_patterns.Select(
                item => item.Value
            )
        ).ToList(); */
        foreach(var item in new_patterns) {
            if (item.Value is Pattern pattern) {
                append_known_pattern(pattern);
            }
        }
    }
}
}