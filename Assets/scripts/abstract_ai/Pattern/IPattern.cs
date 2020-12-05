

using System.Collections.Generic;

namespace rvinowise.ai.patterns {
public interface IPattern {

    string id {
        get;
    }


    void add_appearance();

    IReadOnlyList<IPattern_appearance> appearances {get;}

}
}