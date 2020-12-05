using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using rvinowise.unity.ai.action;
using UnityEngine;
using rvinowise.unity.extensions;
using Action = rvinowise.unity.ai.action.Action;
using rvinowise.ai.action;
using rvinowise.ai.patterns;

namespace rvinowise.unity.ai.patterns {

public class Pattern_finder: IPattern_finder
{
    private ISet<IPattern> checked_patterns;

    private IHistory_interval action_history;


    public void find_new_patterns(
        int begin, int end
    ) {
        ISet<IPattern> familiar_patterns = find_familiar_patterns(interval);
        foreach (IPattern pattern in familiar_patterns)
        {
            
        }
    }

    private ISet<IPattern> find_familiar_patterns(
        IHistory_interval interval
    ) {
        ISet<IPattern> result = new HashSet<IPattern>(); 
        foreach (IAction_group group in interval) {
            foreach (IAction action in group) {
                result.Add(action.pattern);
            }
        }

        return result;
    }

    public IReadOnlyList<IPattern_appearance> find_repeated_pairs(
        IReadOnlyList<IPattern_appearance> beginnings
        IReadOnlyList<IPattern_appearance> endings
    ) {

    }

    
    
}





}