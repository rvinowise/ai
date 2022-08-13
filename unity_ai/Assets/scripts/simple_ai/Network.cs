using rvinowise.ai.general;

using TFigure = rvinowise.ai.simple.Figure;

namespace rvinowise.ai.simple {

public class Network: 
    INetwork<TFigure> // where TFigure: class?, IFigure
{
    
    #region INetwork

    public IAction_history action_history { get; }
    public IFigure_provider<TFigure> figure_provider { get; }
    public ISequence_finder<TFigure> sequence_finder { get; }
    
    public void input_signal(string id) {
        action_history.input_signals(
            new[] {figure_provider.find_figure_with_id(id)}    
        );
    }
    
    #endregion INetwork

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
        IFigure_provider<TFigure> figure_provider,
        ISequence_finder<TFigure> sequence_finder
    ) {
        this.action_history = action_history;
        this.figure_provider = figure_provider;
        this.sequence_finder = sequence_finder;
    }

    public static INetwork<TFigure> get_empty_network() {
        return new Network();
    }
    
    public static INetwork<TFigure> get_network_with_base_signals() {
        INetwork<TFigure> network = new Network();
        new Base_signals<TFigure>(network.figure_provider);
        return network;
    }

    


    private static Figure create_simple_figure(string id) {
        return new simple.Figure(id);
    }

    

 

}


}