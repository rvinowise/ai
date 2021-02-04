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
MonoBehaviour,
IPattern_finder
{
    public Pattern pattern_preafab;
    public Action_history action_history;
    public Pattern_storage pattern_storage;
    //private ISet<IPattern> checked_patterns;

    private IDictionary<string, IPattern> found_patterns = 
        new Dictionary<string, IPattern>();

    private IReadOnlyList<IAction_group> action_groups;

    public void enrich_pattern_storage() {
        var patterns = get_new_patterns(
            action_history.get_action_groups(
                0,
                action_history.last_moment
            )
        );
        pattern_storage.append_patterns(patterns);
    }

    public 
    IDictionary<string, IPattern>
    get_new_patterns(
        IReadOnlyList<IAction_group> in_action_groups
    ) {
        action_groups = in_action_groups;
        ISet<IPattern> familiar_patterns = find_familiar_patterns(
            action_groups
        );
        var result = new Dictionary<string, IPattern>();
        foreach (IPattern beginning_pattern in familiar_patterns)
        {
            
            result = result.Union(
                find_new_patterns_starting_with(
                    beginning_pattern,
                    familiar_patterns
                )
            ).ToDictionary(k=>k.Key, v=>v.Value);
        }

        return result;
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

    private IDictionary<string, IPattern> 
    find_new_patterns_starting_with(
        IPattern beginning_pattern,
        ISet<IPattern> familiar_patterns
    ) {
        IDictionary<string, IPattern> new_patterns = 
            new Dictionary<string, IPattern>();
        IReadOnlyList<IPattern_appearance> appearances_of_beginning = 
            beginning_pattern.get_appearances_in_interval(
                action_groups.First().moment,
                action_groups.Last().moment
            );
        foreach (IPattern ending_pattern in familiar_patterns)
        {
            if (!is_possible_pattern(beginning_pattern, ending_pattern)) {
                continue;
            }
            var appearances_of_ending = 
                ending_pattern.get_appearances_in_interval(
                    action_groups.First().moment,
                    action_groups.Last().moment
                );
            if (check_if_pattern_appeared(
                    appearances_of_beginning,
                    appearances_of_ending
                ) is IPattern new_pattern
            ) {
                new_patterns.Add(
                    new_pattern.id,
                    new_pattern
                );
            }
        }
        return new_patterns;

        
    }

    private bool is_possible_pattern(
        IPattern beginning,
        IPattern ending
    ) {
        if (beginning == ending) {
            return false;
        }
        IPattern checked_pattern = pattern_preafab.get_for_repeated_pair(
            beginning,
            ending
        );
        if (pattern_is_known(checked_pattern)) {
            return false;
        }
        return true;
    }

    private bool pattern_is_known(IPattern pattern) {

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
    public IPattern check_if_pattern_appeared(
        IReadOnlyList<IPattern_appearance> beginnings,
        IReadOnlyList<IPattern_appearance> endings
    ) {
        IPattern imagined_pattern = pattern_preafab.get_for_repeated_pair(
            beginnings.First().pattern,
            endings.First().pattern
        );

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
                imagined_pattern.create_appearance(
                    closest_beginning.appearance.start.action_group,
                    potential_ending.end.action_group
                );
                i_next_beginning = closest_beginning.index + 1;
            }
            
            i_ending++;
        }

        if (pattern_appeared_at_least_twice(imagined_pattern)) {
            return imagined_pattern;
        } else {
            ((Pattern)imagined_pattern).destroy();
        }
        return null;

        bool pattern_appeared_at_least_twice(IPattern pattern) {
            return pattern.get_appearances_in_interval(
                0,
                action_groups.Last().moment
            ).Count >= 2;
        }
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