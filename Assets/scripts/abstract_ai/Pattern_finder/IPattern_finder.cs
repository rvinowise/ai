

using System.Collections.Generic;

namespace rvinowise.ai.patterns {
public interface IPattern_finder {


     
    IDictionary<string, IPattern>
    get_new_patterns(
        IReadOnlyList<IAction_group> in_action_groups
    );


    

}
}