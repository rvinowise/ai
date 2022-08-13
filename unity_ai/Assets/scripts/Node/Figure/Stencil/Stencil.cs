using System.Collections.Generic;
using rvinowise.ai.general;


namespace rvinowise.ai.unity {

public class Stencil: 
    Figure_representation,
    IStencil
{
    
    List<Subfigure> outputs = new List<Subfigure>();
    List<Subfigure> inputs = new List<Subfigure>();
    
    #region IStencil

    public IReadOnlyList<ISubfigure> get_outputs() => outputs.AsReadOnly();
    public IReadOnlyList<ISubfigure> get_inputs() => inputs.AsReadOnly();

    #endregion IStencil

    
}
}