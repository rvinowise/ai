using System;
using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.general;
using UnityEngine;

namespace rvinowise.ai.unity.persistence.serializable {

[Serializable]
public class Network {
    public List<Action_group> action_groups = new List<Action_group>();
    public List<Action> actions = new List<Action>();
    public List<Figure> figures = new List<Figure>();
    public List<Pattern> patterns = new List<Pattern>();
    public List<Subfigure> subfigures = new List<Subfigure>();
    public List<Figure_appearance> figure_appearances = new List<Figure_appearance>();
}
[Serializable]
public class Action_group {
    public string moment;
    //public List<string> actions = new List<string>();
    
    #region visualisation
    Position position;
    #endregion

    public Action_group( general.IAction_group action_group) {
        moment = action_group.moment.ToString();
        
        if (action_group is unity.Action_group unity_group) {
            position = new Position(unity_group.transform.position);
        }
    }
}

[Serializable]
public class Action {
    public int moment;
    public bool is_end;
    public string figure_appearance;

    // public Action(general.IAction action) {
    //     moment = action.
    // }
}

[Serializable]
public class Figure {
    public string id;
    public List<string> subfigures = new List<string>();
    
    #region visualisation
    public Position position;
    #endregion

    public Figure(unity.Figure figure) {
        id = figure.id;
        foreach(ISubfigure ai_subfigure in figure.subfigures) {
            subfigures.Add(ai_subfigure.id);
        }
        position = new Position(figure.transform.position);
    }
  
}

[Serializable]
public class Subfigure {
    public string id;
    public string parent_figure;
    public string referenced_figure;
    public List<string> previous_subfigures = new List<string>();
    public List<string> next_subfigures = new List<string>();
    
    #region visualisation
    public Position position;
    #endregion

    public Subfigure(general.ISubfigure ai_subfigure) {
        id = ai_subfigure.id;
        parent_figure = ai_subfigure.parent.id;
        referenced_figure = ai_subfigure.figure.id;
        foreach(ISubfigure previous_subfigure in ai_subfigure.previous) {
            previous_subfigures.Add(previous_subfigure.id);
        }
        foreach(ISubfigure next_subfigure in ai_subfigure.next) {
            next_subfigures.Add(next_subfigure.id);
        }
        if (ai_subfigure is unity.Subfigure unity_sunfigure) {
            position = new Position(unity_sunfigure.transform.position);
        }
    }
}
[Serializable]
public class Pattern {
    public string id;
    public List<string> subfigures = new List<string>(); //actual figures and patterns

    public Pattern(rvinowise.ai.general.IPattern pattern) {
        id = pattern.id;
        foreach(IFigure subfigure in pattern.subfigures) {
            subfigures.Add(subfigure.id);
        }
    }
}
[Serializable]
public class Figure_appearance {
    public string appeared_figure;
    public string start_moment;
    public string end_moment;

    public Figure_appearance(general.IFigure_appearance appearance) {
        
        appeared_figure = appearance.figure.id;
        start_moment = appearance.start_moment.ToString();
        end_moment = appearance.end_moment.ToString();
    }
}

[Serializable]
public class Position {
    public float x;
    public float y;
    public float z;

    public Position(UnityEngine.Vector3 unity_vector) {
        x = unity_vector.x;
        y = unity_vector.y;
        z = unity_vector.z;
    }
}

}