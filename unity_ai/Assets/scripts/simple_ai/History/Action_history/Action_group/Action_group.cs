using System.Collections;
using System.Collections.Generic;
using System.Linq;
using rvinowise.ai.general;
using System.Numerics;


namespace rvinowise.ai.simple {

public class Action_group:
IAction_group 
{

    public IEnumerable<IAction> get_actions() =>
        actions;


    public BigInteger moment { get; private set; }

    public float mood {
        get;
        private set;
    }

    public void init_mood(float value) {
        mood = value;
    }

    private readonly IList<IAction> actions = new List<IAction>();
    

    public Action_group(
        BigInteger moment,
        float mood = 0f
    ) {
        this.moment = moment;
        this.mood = mood;
    }

    

    public void add_action(IAction in_action) {
        actions.Add(in_action);
    }

    public void remove_action(IAction in_action) {
        actions.Remove(in_action);
    }


    public bool has_action(IFigure figure, Action_type type) =>
        actions.Any(action => action.figure == figure && action.type == type);


    
 }
}