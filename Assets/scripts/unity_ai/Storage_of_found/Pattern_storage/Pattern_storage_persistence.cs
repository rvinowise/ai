using System;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.unity.extensions;
using rvinowise.ai.unity.persistence;
using rvinowise.unity.ui.table;
using UnityEngine;
using UnityEngine.UI;

namespace rvinowise.ai.unity {
public partial class Pattern_storage {
    Persistent persistent;

    private void prepare_to_saving(Persistent_state persistent_state)
    {
        List<string> pattern_ids = new List<string>();
        foreach (IPattern pattern in known_patterns) {
            if (pattern is Pattern unity_pattern) {
                string pattern_guid = 
                    unity_pattern.GetComponent<Persistent>().persistent_state.guid;
                pattern_ids.Add(pattern_guid);
            }
        }
        persistent_state.genericValues["Pattern_storage.known_patterns"] = pattern_ids;
    }
}
}