

using System.Collections.Generic;

namespace rvinowise.ai.general {
public interface ISequence_finder {

    // IDictionary<string, IFigure>
    // get_new_sequences(
    //     IReadOnlyList<IAction_group> in_action_groups
    // );
    void init_unity_fields(
        IAction_history action_history,
        IFigure_storage figure_storage,
        ISequence_builder sequence_builder
    );
    void enrich_storage_with_sequences();



}
}