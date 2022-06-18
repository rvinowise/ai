using UnityEngine;
using System;
using rvinowise.ai.general;

using TFigure = rvinowise.ai.simple.Figure;

namespace rvinowise.ai.simple {

public class Network: 
    INetwork<TFigure> // where TFigure: class?, IFigure
{

    public IAction_history action_history { get; }
    public ISequence_finder<TFigure> sequence_finder { get; }

    public IFigure_provider<TFigure> figure_provider { get; }

    public Network() {
        action_history = new simple.Action_history();
        figure_provider = new Figure_provider<TFigure>(create_simple_figure);
        sequence_finder = new Sequence_finder<TFigure>(
            action_history,
            figure_provider
        );
    }
    
    public Network(
        IAction_history action_history,
        ISequence_finder<TFigure> sequence_finder
    ) {
        this.action_history = action_history;
        this.sequence_finder = sequence_finder;
    }

    public static INetwork<TFigure> get_empty_network() {
        return new Network();
    }
    
    public static INetwork<TFigure> get_network_with_base_signals() {
        INetwork<TFigure> network = new Network();
        network.fill_figure_storage_with_base_signals();
        return network;
    }

    #region INetwork


    
    #endregion INetwork
    
    
    
    public Figure create_simple_figure(string id) {
        return new simple.Figure(id);
    }

    public void input_signal(string id) {
        action_history.input_signals(
            new[] {figure_provider.find_figure_with_id(id)}    
        );
    }

    public void fill_figure_storage_with_base_signals() {
        IBase_signals_initializer figure_provider_initialiser = 
            new Base_signals_initializer<TFigure>(figure_provider);
        figure_provider_initialiser.create_base_signals();
    }

 

}


}