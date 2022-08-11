namespace rvinowise.ai.general {


public enum Action_type {
    Start,
    End
}

public interface IAction  {

    Action_type type { get; }
    
    IFigure figure{get;}
    IFigure_appearance figure_appearance{get;}
    
    IAction_group action_group { get; }
    
}

}