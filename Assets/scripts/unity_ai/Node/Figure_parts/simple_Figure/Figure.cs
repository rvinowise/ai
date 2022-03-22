using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.general;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Linq;
using rvinowise.ai.unity.visuals;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.ui.input;
using rvinowise.unity.ui.input.mouse;
using TMPro;

namespace rvinowise.ai.simple {

public class Figure: 
IFigure
{
    private readonly List<IFigure_representation> representations 
        = new List<IFigure_representation>();

    private readonly List<IFigure_appearance> _appearances 
        = new List<IFigure_appearance>();
    
    
    #region building

    public Figure(string id) {
        this.id=id;
    }

    public IFigure_representation create_representation() {
        IFigure_representation representation = new Figure_representation();
        representations.Add(representation);
        return representation;
    }

    #endregion

    #region IFigure

    public string id {
        get;
        set;
    }
    public IReadOnlyList<IFigure_appearance> get_appearances() => _appearances;

    public IReadOnlyList<IFigure_appearance> get_appearances_in_interval(
        BigInteger start, BigInteger end
    ) {
        return _appearances.Where(
            appearance => 
                (appearance.start_moment >= start) &&
                (appearance.end_moment <= end)
        ).ToList().AsReadOnly();
    }

    public void add_appearance(IFigure_appearance appearance) {
        _appearances.Add(appearance);
        
    }

    public IReadOnlyList<IFigure_representation> get_representations() 
        => representations.AsReadOnly();
    #endregion IFigure
    
    
    #region sequential figure
    private List<IFigure> sequence = new List<IFigure>();

    public void set_lowlevel_sequence(IEnumerable<IFigure> in_sequence) {
        foreach(IFigure figure in in_sequence){
            sequence.Add(figure); 
        }
    }
    public IReadOnlyList<IFigure> as_lowlevel_sequence() {
        if (sequence.Any()) {
            return sequence.AsReadOnly();
        }
        return new List<IFigure> {this};
    }

    public bool is_sequential() {
        return sequence.Any();
    }
    #endregion sequential figure



        
}
}