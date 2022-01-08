using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.unity;
using UnityEngine;
using rvinowise.unity.extensions;
using Action = rvinowise.ai.unity.Action;
using rvinowise.rvi.contracts;
using rvinowise.ai.general;
using System.Numerics;
using rvinowise.ai.general;

namespace rvinowise.ai.unity {

public class Pattern_finder:
MonoBehaviour
{
    
    public Action_history action_history;
    public Pattern_storage pattern_storage;

    private IDictionary<string, IPattern> found_patterns = 
        new Dictionary<string, IPattern>();

    private IReadOnlyList<IAction_group> action_groups;

    public void enrich_pattern_storage() {
        action_groups = action_history.get_action_groups(
            0,
            action_history.last_moment
        );
        ISet<IFigure> familiar_figures = find_familiar_figures(
            action_groups
        );
        foreach (IFigure beginning_figure in familiar_figures)
        {
            find_new_patterns_starting_with(
                beginning_figure,
                familiar_figures
            );
        }
    }


    private ISet<IFigure> find_familiar_figures(
        IReadOnlyList<IAction_group> action_groups
    ) {
        ISet<IFigure> result = new HashSet<IFigure>(); 
        foreach (IAction_group group in action_groups) {
            foreach (IAction action in group) {
                result.Add(action.figure);
            }
        }

        return result;
    }

    private void find_new_patterns_starting_with(
        IFigure beginning_figure,
        ISet<IFigure> familiar_figures
    ) {
        foreach (IFigure ending_figure in familiar_figures)
        {
            if (!is_possible_pattern(beginning_figure, ending_figure)) {
                continue;
            }

            IPattern signal_pair = pattern_storage.get_pattern_for_pair(
                beginning_figure,
                ending_figure
            );
            IReadOnlyList<IFigure_appearance> appearances_of_beginning 
            = get_unused_in_beginning_appearances_in_interval(
                action_groups.First().moment,
                action_groups.Last().moment,
                beginning_figure,
                signal_pair
            );
            var appearances_of_ending = 
            get_unused_in_ending_appearances_in_interval(
                action_groups.First().moment,
                action_groups.Last().moment,
                ending_figure,
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

    IReadOnlyList<IFigure_appearance> get_unused_in_beginning_appearances_in_interval(
        BigInteger start, 
        BigInteger end,
        IFigure figure_used_in_beginning,
        IPattern user_pattern
    ) {
        var child_appearances = figure_used_in_beginning.get_appearances_in_interval(
            start, 
            end
        );
        var user_appearances = user_pattern.get_appearances_in_interval(
            start, 
            end
        );
        
        IReadOnlyList<IFigure_appearance> result = 
        child_appearances.Where(
            child_appearance => 
            (
                user_appearances.All(
                    user_appearance => 
                        user_appearance.start_moment != child_appearance.start_moment
                        )
                )
        ).ToList();

        
        return result;
    }
    IReadOnlyList<IFigure_appearance> get_unused_in_ending_appearances_in_interval(
        BigInteger start, 
        BigInteger end,
        IFigure figure_used_in_ending,
        IPattern user_pattern
    ) {
        var child_appearances = figure_used_in_ending.get_appearances_in_interval(
            start, 
            end
        );
        var user_appearances = user_pattern.get_appearances_in_interval(
            start, 
            end
        );
        
        IReadOnlyList<IFigure_appearance> result = child_appearances.Where(
            child_appearance => 
            (
                user_appearances.All(
                    user_appearance => user_appearance.end_moment != child_appearance.end_moment
                    )
                )
        ).ToList();

        
        return result;
    } 


    private bool is_possible_pattern(
        IFigure beginning,
        IFigure ending
    ) {
        if (beginning == ending) {
            return false;
        }
        
        return true;
    }

    private struct Appearance_in_list {
        public int index;
        public IFigure_appearance appearance;

        public Appearance_in_list(
            IFigure_appearance in_appearance,
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
        IReadOnlyList<IFigure_appearance> beginnings,
        IReadOnlyList<IFigure_appearance> endings
    ) {
        
        int i_ending = 0;
        int i_next_beginning = 0;
        while (i_ending < endings.Count) {
            var potential_ending = endings[i_ending++];
            
            var closest_beginning = find_appearance_closest_to_moment(
                beginnings,
                i_next_beginning,
                potential_ending.start_moment
            );
            if (!closest_beginning.is_found()) {
                continue;
            }
            if (same_patterns_exist_inside(
                signal_pair,
                closest_beginning.appearance.start_moment,
                potential_ending.end_moment
            )) {
                continue;
            }
            signal_pair.create_appearance(
                closest_beginning.appearance,
                potential_ending
            );
            i_next_beginning = closest_beginning.index + 1;
        }

        if (!pattern_appeared_at_least_twice(signal_pair)) {
            pattern_storage.remove_pattern(signal_pair);
            ((Pattern)signal_pair).destroy();
        }

        bool same_patterns_exist_inside(
            IPattern pattern,
            BigInteger start,
            BigInteger end
        ) {
            return 
            pattern.get_appearances_in_interval(start, end).Any();
        }
        
    }

    private bool pattern_appeared_at_least_twice(IPattern pattern) {
        return pattern.get_appearances_in_interval(
            0,
            action_groups.Last().moment
        ).Count >= 2;
    }

    

    Appearance_in_list find_appearance_closest_to_moment(
        IReadOnlyList<IFigure_appearance> appearances,
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