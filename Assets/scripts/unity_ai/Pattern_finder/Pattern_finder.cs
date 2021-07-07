using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using rvinowise.unity.ai.action;
using UnityEngine;
using rvinowise.unity.extensions;
using Action = rvinowise.unity.ai.action.Action;
using rvinowise.rvi.contracts;
using rvinowise.ai.patterns;
using System.Numerics;

namespace rvinowise.unity.ai {

public class Pattern_finder:
MonoBehaviour
//,IPattern_finder
{
    public Pattern pattern_preafab;
    public Action_history action_history;
    public Pattern_storage pattern_storage;
    //private ISet<IPattern> checked_patterns;

    private IDictionary<string, IPattern> found_patterns = 
        new Dictionary<string, IPattern>();

    private IReadOnlyList<IAction_group> action_groups;

    public void enrich_pattern_storage() {
         action_groups = action_history.get_action_groups(
            0,
            action_history.last_moment
        );
        ISet<IPattern> familiar_patterns = find_familiar_patterns(
            action_groups
        );
        foreach (IPattern beginning_pattern in familiar_patterns)
        {
            find_new_patterns_starting_with(
                beginning_pattern,
                familiar_patterns
            );
        }
    }


    private ISet<IPattern> find_familiar_patterns(
        IReadOnlyList<IAction_group> action_groups
    ) {
        ISet<IPattern> result = new HashSet<IPattern>(); 
        foreach (IAction_group group in action_groups) {
            foreach (IAction action in group) {
                result.Add(action.pattern);
            }
        }

        return result;
    }

    private void find_new_patterns_starting_with(
        IPattern beginning_pattern,
        ISet<IPattern> familiar_patterns
    ) {

        
        foreach (IPattern ending_pattern in familiar_patterns)
        {
            if (!is_possible_pattern(beginning_pattern, ending_pattern)) {
                continue;
            }

            IPattern signal_pair = get_pattern_for_pair(
                beginning_pattern,
                ending_pattern
            );
            IReadOnlyList<IPattern_appearance> appearances_of_beginning = 
            get_unused_in_beginning_appearances_in_interval(
                action_groups.First().moment,
                action_groups.Last().moment,
                beginning_pattern,
                signal_pair
            );
            var appearances_of_ending = get_unused_in_ending_appearances_in_interval(
                action_groups.First().moment,
                action_groups.Last().moment,
                ending_pattern,
                signal_pair
            );
            if (
                appearances_of_beginning.Any() && 
                appearances_of_ending.Any()
            ) {
                save_pattern_appearances(
                    signal_pair,
                    appearances_of_beginning,
                    appearances_of_ending
                );
            }
        }

        
    }

    IReadOnlyList<IPattern_appearance> get_unused_in_beginning_appearances_in_interval(
        BigInteger start, 
        BigInteger end,
        IPattern pattern_used_in_beginning,
        IPattern user_pattern
    ) {
        var child_appearances = pattern_used_in_beginning.get_appearances_in_interval(
            start, 
            end
        );
        var user_appearances = user_pattern.get_appearances_in_interval(
            start, 
            end
        );
        
        IReadOnlyList<IPattern_appearance> result = child_appearances.Where(
            child_appearance => 
            (
                !user_appearances.Any(
                    user_appearance => user_appearance.start_moment == 
                    child_appearance.start_moment
                )
            )
        ).ToList<IPattern_appearance>();

        
        return result;
    }
    IReadOnlyList<IPattern_appearance> get_unused_in_ending_appearances_in_interval(
        BigInteger start, 
        BigInteger end,
        IPattern pattern_used_in_ending,
        IPattern user_pattern
    ) {
        var child_appearances = pattern_used_in_ending.get_appearances_in_interval(
            start, 
            end
        );
        var user_appearances = user_pattern.get_appearances_in_interval(
            start, 
            end
        );
        
        IReadOnlyList<IPattern_appearance> result = child_appearances.Where(
            child_appearance => 
            (
                !user_appearances.Any(
                    user_appearance => user_appearance.end_moment ==
                    child_appearance.end_moment
                )
            )
        ).ToList<IPattern_appearance>();

        
        return result;
    } 


    private bool is_possible_pattern(
        IPattern beginning,
        IPattern ending
    ) {
        if (beginning == ending) {
            return false;
        }
        
        return true;
    }

    private struct Appearance_in_list {
        public int index;
        public IPattern_appearance appearance;

        public Appearance_in_list(
            IPattern_appearance in_appearance,
            int in_index
        ) {
            appearance = in_appearance;
            index = in_index;
        }
        public bool is_found() {
            return appearance != null;
        }
    }
    public void save_pattern_appearances(
        IPattern signal_pair,
        IReadOnlyList<IPattern_appearance> beginnings,
        IReadOnlyList<IPattern_appearance> endings
    ) {
 
        int i_ending = 0;
        int i_next_beginning = 0;
        while (i_ending < endings.Count) {
            var potential_ending = endings[i_ending];
            
            var closest_beginning = find_appearance_closest_to_moment(
                beginnings,
                i_next_beginning,
                potential_ending.start_moment
            );
            if (closest_beginning.is_found()) {
                signal_pair.create_appearance(
                    closest_beginning.appearance.start.action_group,
                    potential_ending.end.action_group
                );
                i_next_beginning = closest_beginning.index + 1;
            }
            
            i_ending++;
        }

        if (!pattern_appeared_at_least_twice(signal_pair)) {
            pattern_storage.remove_pattern(signal_pair);
            ((Pattern)signal_pair).destroy();
        }

        
    }

    private bool pattern_appeared_at_least_twice(IPattern pattern) {
        return pattern.get_appearances_in_interval(
            0,
            action_groups.Last().moment
        ).Count >= 2;
    }

    IPattern get_pattern_for_pair(
        IPattern beginning,
        IPattern ending
    ) {
        if (
            pattern_storage.get_pattern_having(beginning, ending)
            is IPattern old_pattern
        ) {
            return old_pattern;
        }
        IPattern new_pattern = pattern_preafab.get_for_repeated_pair(
            beginning,
            ending
        );
        pattern_storage.append_pattern(new_pattern);
        return new_pattern;
    }

    Appearance_in_list find_appearance_closest_to_moment(
        IReadOnlyList<IPattern_appearance> appearances,
        int start_index,
        BigInteger moment
    ) {
        Appearance_in_list result = new Appearance_in_list(null,-1);
        for (int i=start_index;i<appearances.Count;i++) {
            var appearance = appearances[i];
            if (appearance.end_moment < moment) {
                result = new Appearance_in_list(appearance, i);
            } else {
                break;
            }
        }
        return result;
    }


}





}