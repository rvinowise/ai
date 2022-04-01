
using System.Collections.Generic;

namespace rvinowise.ai.general {
public interface ISequence_builder {

    // public IFigure add_sequential_representation_for_pair(
    //     IFigure figure,
    //     IFigure beginning,
    //     IFigure ending
    // );

    public IFigure add_sequential_representation(
        IFigure figure,
        IReadOnlyList<IFigure> subfigures
    );

    public IReadOnlyList<IFigure> get_sequence_of_subfigures_from(
        IFigure beginning, IFigure ending
    );
}
}