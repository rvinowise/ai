
using System.Collections.Generic;

namespace rvinowise.ai.general {
public interface ISequence_builder {

    public IFigure create_sequence_for_pair(
        IFigure beginning,
        IFigure ending
    );

    public IFigure create_figure_for_sequence_of_subfigures(
        IReadOnlyList<IFigure> subfigures
    );

    public IReadOnlyList<IFigure> get_sequence_of_subfigures_from(
        IFigure beginning, IFigure ending
    );
}
}