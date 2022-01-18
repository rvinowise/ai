

using System.Collections.Generic;

namespace rvinowise.ai.general {
public interface ISequence_finder {


     
    IDictionary<string, IFigure>
    get_new_sequences(
        IReadOnlyList<IAction_group> in_action_groups
    );


    

}
}