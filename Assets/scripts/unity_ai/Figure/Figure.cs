using System.Collections.Generic;
using System.Numerics;
using abstract_ai;
using UnityEngine;

namespace rvinowise.unity.ai.figure {

public class Figure: 
MonoBehaviour,
IFigure {
    
    public IReadOnlyList<ISubfigure> first_subfigures {
        get; private set;
    } = new List<ISubfigure>();

    public bool selected {
        get { return _selected; }
        set {
            _selected = value;
            //animator.SetBool("selected", _selected);
            //this.set_appearances_are_highlighted(selected);
        }
    }
    private bool _selected = false;

    #region IFigure

    public string id { get; }

    public string as_dot_graph() {
        throw new System.NotImplementedException();
    }

    public IReadOnlyList<IFigure_appearance> get_appearances_in_interval(BigInteger start, BigInteger end) {
        throw new System.NotImplementedException();
    }
    #endregion IFigure
    

    #region building

    
    #endregion
    
}
}