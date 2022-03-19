/* visualises all the actions that were input into the system */
using System.Collections.Generic;
using rvinowise.ai.unity;
using UnityEngine;
using rvinowise.unity.extensions;
using Action = rvinowise.ai.unity.Action;
using rvinowise.ai.general;
using System.Linq;
using System.Numerics;
using rvinowise.rvi.contracts;
using rvinowise.unity.ui.input;
using System;

namespace rvinowise.ai.simple {
public class Action_history:
IAction_history {

    private Sequence_builder sequence_builder;
    private readonly ISet<IFigure> known_figures = new HashSet<IFigure>();
    
    #region IAction_history
    public BigInteger last_moment {get; private set;}

    public IReadOnlyList<IAction_group> get_action_groups(
        BigInteger begin, 
        BigInteger end
    ) {
        List<IAction_group> result = action_groups.Where(
            action_group => 
            (action_group.moment >= begin) &&
            (action_group.moment <= end)
        ).ToList();

        return result.AsReadOnly();
    }

    IFigure_appearance IAction_history.create_figure_appearance(
        IFigure figure, IAction_group start, IAction_group end
    ) => create_figure_appearance(figure, start, end);

    private IFigure_appearance create_simple_figure_appearance(
        IFigure figure,
        IAction_group start,
        IAction_group end
    ) {
        Contract.Requires(
            start.moment < end.moment,
            "should have a positive time interval"
        );
        IFigure_appearance appearance = 
            new Figure_appearance(
                figure, start, end
            );
        figure.add_appearance(appearance);
        start.add_action(appearance.get_start());
        end.add_action(appearance.get_end());
        return appearance;
    }

    public void input_signals(IEnumerable<IFigure> signals, int mood_change =0) {
        float new_mood = get_last_mood() + mood_change;
        // IAction_group start_group = create_next_action_group(new_mood);
        // IAction_group end_group = create_next_action_group(new_mood);
        IAction_group action_group = create_next_action_group(new_mood);
        create_figure_appearances(
            signals,
            action_group,
            action_group
        );
    }
    
    public IFigure find_figure_having_sequence(
        IReadOnlyList<IFigure> subfigures
    ) {
        foreach(var figure in known_figures) {
            if (
                figure.as_lowlevel_sequence().SequenceEqual(subfigures)
            ) {
                return figure;
            }
        }
        return null;
    }
    public IFigure provide_sequence_for_pair(
        IFigure beginning,
        IFigure ending
    ) {
        var subfigures = sequence_builder.get_sequence_of_subfigures_from(
            beginning, ending
        );
        return provide_figure_having_sequence(subfigures);
    }
    
    #endregion IAction_history

    #region used by derived

    public void add_next_action_group(
        IAction_group new_group
    ) {
        last_moment++;
        action_groups.Add(new_group);
        moments_to_action_groups.Add(last_moment, new_group);
    }

    public Func<
        IFigure, IAction_group, IAction_group,
        IFigure_appearance
    > create_figure_appearance;

    public delegate IAction_group Create_next_action_group(float in_mood = 0f);

    public Create_next_action_group create_next_action_group;
    
    #endregion used by derived

    public Action_history() {
        create_figure_appearance = create_simple_figure_appearance;
        create_next_action_group = create_next_simple_action_group;
        sequence_builder = new Sequence_builder();
    }

    private IList<IAction_group> action_groups = 
        new List<IAction_group>();
    
    private Dictionary<BigInteger, IAction_group> moments_to_action_groups=
        new Dictionary<BigInteger, IAction_group>();

    private Dictionary_of_lists<
        IFigure,
        IFigure_appearance
    > figure_appearances =
        new Dictionary_of_lists<
            IFigure,
            IFigure_appearance
        >();

    
    
    public IAction_group create_next_simple_action_group(float in_mood = 0f) {
        IAction_group new_group =
            new Action_group(
                last_moment+1,
                in_mood
            );
        add_next_action_group(new_group);
        return new_group;
    }
    
    
    private void create_figure_appearances(
        IEnumerable<IFigure> figures,
        IAction_group start,
        IAction_group end
    ) {
        foreach (var figure in figures) {
            create_figure_appearance(figure, start, end);
        }
    }
    
    private float get_last_mood() {
        if (action_groups.Any()) {
            return action_groups.Last().mood;
        }
        return 0f;
    }

    
    public IAction_group get_action_group_at_moment(
        BigInteger moment
    ) {
        moments_to_action_groups.TryGetValue(moment,out var result);
        return result;
    }

    private IFigure provide_figure_having_sequence(
        IReadOnlyList<IFigure> subfigures
    ) {
        if (find_figure_having_sequence(subfigures) is IFigure old_pattern) {
            return old_pattern;
        }
        IFigure new_figure =  sequence_builder.create_figure_for_sequence_of_subfigures(subfigures);
        
        return new_figure;
    }

    public void remove_appearances_of(IFigure figure) {
        foreach (IFigure_appearance appearance in figure.get_appearances()) {
            appearance.get_start().action_group.remove_action(appearance.get_start());
            appearance.get_end().action_group.remove_action(appearance.get_end());
        }
    }
}
}