using System;
using System.Collections.Generic;
using rvinowise.unity.extensions;
using UnityEngine;

namespace rvinowise.unity.ai.patterns {
public class Pattern_storage: MonoBehaviour {
    public Pattern pattern_prefab;
    public List<Pattern> known_patterns;
    public Canvas pattern_table;
    
    void Awake() {
        init_test_patterns(100);
    }

    private void init_test_patterns(int qty) {
        for (int i=0;i<qty;i++) {
            Pattern pattern = pattern_prefab.get_from_pool<Pattern>();
            pattern.id = get_id_for_index(i);
            pattern.name = String.Format("pattern {0}", pattern.id);
            append_known_pattern(pattern);
        }
    }

    private string get_id_for_index(int in_index) {
        return String.Format("{0}",in_index);
    }
    private void append_known_pattern(Pattern in_pattern) { 
        known_patterns.Add(in_pattern);
        
        pattern_table.add_item(in_pattern);
    }
}
}