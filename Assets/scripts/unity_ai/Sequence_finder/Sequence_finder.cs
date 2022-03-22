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
using rvinowise.unity;

namespace rvinowise.ai.unity {

public class Sequence_finder:
ISequence_finder
{
    
    private IAction_history action_history;
    private IFigure_provider figure_provider;
    
    #region exposed to unity editor
    [SerializeField] private Action_history _action_history;
    [SerializeField] private Figure_provider _figure_provider;
    #endregion exposed to unity editor
    
    private IDictionary<string, IFigure> found_patterns = 
        new Dictionary<string, IFigure>();

    private IReadOnlyList<IAction_group> action_groups;

 

    public Sequence_finder(
        IAction_history action_history,
        IFigure_provider figure_provider
    ) {
        this.action_history = action_history;
        this.figure_provider = figure_provider;
    }


    public void enrich_storage_with_sequences() {
        action_groups = action_history.get_action_groups(
            0,
            action_history.last_moment
        );
        ISet<IFigure> familiar_figures = find_familiar_figures(
            action_groups
        );
        foreach (IFigure beginning_figure in familiar_figures)
        {
            find_new_sequences_starting_with(
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

    private void find_new_sequences_starting_with(
        IFigure beginning_figure,
        ISet<IFigure> familiar_figures
    ) {
        foreach (IFigure ending_figure in familiar_figures)
        {
            if (!is_possible_sequence(beginning_figure, ending_figure)) {
                continue;
            }
  
            IFigure signal_pair = figure_provider.provide_sequence_for_pair(
                beginning_figure,
                ending_figure
            );
            var appearances_of_beginning = 
                get_unused_in_beginning_appearances_in_interval(
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
                save_sequence_appearances(
                    signal_pair,
                    appearances_of_beginning,
                    appearances_of_ending
                );
            }

            if (!sequence_appeared_at_least_twice(signal_pair)) {
                delete_figure(signal_pair);
            }

        }

        
    }

    private void delete_figure(IFigure figure) {
        action_history.remove_appearances_of(figure);
        figure_provider.remove_figure(figure);
        if (figure is IHave_destructor destructable_figure) {
            destructable_figure.destroy();
        }
    }

    private IReadOnlyList<IFigure_appearance> 
    get_unused_in_beginning_appearances_in_interval(
        BigInteger start, 
        BigInteger end,
        IFigure figure_used_in_beginning,
        IFigure user_sequence
    ) {
        var child_appearances 
        = figure_used_in_beginning.get_appearances_in_interval(
            start, 
            end
        );
        var user_appearances 
        = user_sequence.get_appearances_in_interval(
            start, 
            end
        );
        
        IReadOnlyList<IFigure_appearance> result 
        = child_appearances.Where(
            child_appearance => 
            user_appearances.All(
                user_appearance => 
                    user_appearance.start_moment != child_appearance.start_moment
            )
        ).ToList();

        
        return result;
    }
    private IReadOnlyList<IFigure_appearance> get_unused_in_ending_appearances_in_interval(
        BigInteger start, 
        BigInteger end,
        IFigure figure_used_in_ending,
        IFigure user_sequence
    ) {
        var child_appearances 
        = figure_used_in_ending.get_appearances_in_interval(
            start, 
            end
        );
        var user_appearances
        = user_sequence.get_appearances_in_interval(
            start, 
            end
        );
        
        IReadOnlyList<IFigure_appearance> result = child_appearances.Where(
            child_appearance => 
            user_appearances.All(
                user_appearance => user_appearance.end_moment != child_appearance.end_moment
            )
        ).ToList();

        
        return result;
    } 


    private bool is_possible_sequence(
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

    private void save_sequence_appearances(
        IFigure signal_pair,
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
            if (same_sequences_exist_inside(
                signal_pair,
                closest_beginning.appearance.start_moment,
                potential_ending.end_moment
            )) {
                continue;
            }
            Contract.Assert(
                signal_pair.id.Length > 1, 
                "Pattern consisting of subfigures should have a longer name"
            );
            action_history.create_figure_appearance(
                signal_pair,
                closest_beginning.appearance.get_start().action_group,
                potential_ending.get_end().action_group
            );
            i_next_beginning = closest_beginning.index + 1;
        }

        

        bool same_sequences_exist_inside(
            IFigure sequence,
            BigInteger start,
            BigInteger end
        ) {
            return 
            sequence.get_appearances_in_interval(start, end).Any();
        }
        
    }

    private bool sequence_appeared_at_least_twice(IFigure sequence) {
        return sequence.get_appearances_in_interval(
            0,
            action_groups.Last().moment
        ).Count >= 2;
    }

    

    private Appearance_in_list find_appearance_closest_to_moment(
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