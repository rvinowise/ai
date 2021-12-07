
using abstract_ai;

namespace rvinowise.unity.ai.figure {

public interface ISubfigure_appearance {
    public ISubfigure subfigure {get;}
    public IFigure_appearance parent_appearance{get;}
    public IFigure_appearance figure_appearance{get;}
}

public class Subfigure_appearance:ISubfigure_appearance {
    public ISubfigure subfigure {get;}

    public IFigure_appearance parent_appearance{get;}
    public IFigure_appearance figure_appearance{get;}

}
}