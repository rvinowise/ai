

namespace rvinowise.ai.general {
public interface INetwork<TFigure>
where TFigure: class?, IFigure
{

    public IAction_history action_history { get; }
    public IFigure_provider<TFigure> figure_provider { get; }
    
    public ISequence_finder<TFigure> sequence_finder { get; }

    public void input_signal(string id);

}
}