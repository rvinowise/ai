

using System.Collections.Generic;

namespace rvinowise.ai.general {
public interface INetwork {

    ISequence_finder get_sequence_finder();
    IFigure_provider get_figure_provider();

    public void fill_figure_storage_with_base_signals();

    public void input_signal(string id);

}
}