using UnityEngine;
using System;
using rvinowise.ai.general;

namespace rvinowise.ai.simple {

public class Network: 
    INetwork 
{
    
    private readonly IAction_history action_history;
    public readonly ISequence_finder sequence_finder;

    public readonly IFigure_provider figure_provider;

    public Network() {
        action_history = new simple.Action_history();
        figure_provider = new Figure_provider(create_simple_figure);
        sequence_finder = new Sequence_finder(
            action_history,
            figure_provider
        );
        fill_figure_storage_with_base_signals();
    }
    
    public Network(
        IAction_history action_history,
        ISequence_finder sequence_finder
    ) {
        this.action_history = action_history;
        this.sequence_finder = sequence_finder;
    }

    public static INetwork get_empty_network() {
        return new Network();
    }
    
    public static INetwork get_network_with_base_signals() {
        INetwork network = new Network();
        network.fill_figure_storage_with_base_signals();
        return network;
    }

    #region INetwork

    public ISequence_finder get_sequence_finder() => sequence_finder;
    public IFigure_provider get_figure_provider() => figure_provider;
    
    #endregion INetwork
    
    
    
    public IFigure create_simple_figure(string id) {
        return new simple.Figure(id);
    }

    public void input_signal(string id) {
        action_history.input_signals(
            new[] {figure_provider.find_figure_with_id(id)}    
        );
    }

    public void fill_figure_storage_with_base_signals() {
        IFigure_provider_initialiser figure_provider_initialiser = 
            new Figure_provider_initialiser(figure_provider);
        figure_provider_initialiser.create_base_signals();
    }

 

}


}