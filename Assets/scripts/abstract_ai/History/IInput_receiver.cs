
using System.Collections.Generic;

namespace rvinowise.ai.general {
public interface IInput_receiver {
    
    
    void input_signals(IEnumerable<IFigure> signals, int mood_change = 0);
}
}