using UnityEngine;
using System;
using rvinowise.ai.general;

namespace rvinowise.ai.simple {



public class Network
{
    private IAction_history action_history;
    private ISequence_finder sequence_finder;

    public Network(
        IAction_history action_history,
        ISequence_finder sequence_finder
    ) {
        this.action_history = action_history;
        this.sequence_finder = sequence_finder;
    }

}
}