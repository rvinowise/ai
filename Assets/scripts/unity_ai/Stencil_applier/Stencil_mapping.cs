using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.general;

namespace rvinowise.ai.unity {

public class Mapped_node {
    public ISubfigure source;
    public ISubfigure target;

    public Mapped_node(ISubfigure source, ISubfigure target) {
        this.source = source;
        this.target = target;
    }
}

public class Stencil_mapping {
    
    private readonly IList<Mapped_node> mapped_nodes = new List<Mapped_node>();
    private readonly List<Mapped_node> first_mappings = new List<Mapped_node>();

    public Stencil_mapping() {

    }    

    public void map_node(ISubfigure source, ISubfigure target) {
        Mapped_node mapped_node = new Mapped_node(source, target);
        mapped_nodes.Add(mapped_node);
    }
    
}
}