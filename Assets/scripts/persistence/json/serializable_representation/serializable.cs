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
    public List<Subfigure> subfigures = new List<Subfigure>();
    public List<Figure_appearance> figure_appearances = new List<Figure_appearance>();
}
[Serializable]
public class Action_group {
    public string moment;
    public int mood;
    //public List<string> actions = new List<string>();
    
    #region visualisation
    public Position position;
    #endregion

    public Action_group() { }

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
    
    public List<Figure_representation> representations = new List<Figure_representation>(); 
    #region visualisation
    public Position position;
    #endregion

    public Figure() { }

    public Figure(unity.Figure figure) {
        id = figure.id;
        
        foreach(unity.Figure_representation ai_representation in figure.representations) {
            representations.Add(
                new Figure_representation(ai_representation)
            );
        }
        position = new Position(figure.transform.position);
    }
  
}

[Serializable]
public class Figure_representation {
    //public string id;
    public List<Subfigure> subfigures = new List<Subfigure>();
    public Figure_representation() { }

    public Figure_representation(unity.Figure_representation representation) {
        //id = representation.id;
        foreach(ISubfigure ai_subfigure in representation.get_subfigures()) {
            subfigures.Add(
                new Subfigure(ai_subfigure)
            );
        }
    }
}

[Serializable]
public class Subfigure {
    public string id;
    public string figure_representation;
    public string referenced_figure;
    public List<string> previous_subfigures = new List<string>();
    public List<string> next_subfigures = new List<string>();
    
    #region visualisation
    public Position position;
    #endregion

    public Subfigure() { }

    public Subfigure(general.ISubfigure ai_subfigure) {
        id = ai_subfigure.id;
        figure_representation = ai_subfigure.parent.id;
        referenced_figure = ai_subfigure.referenced_figure.id;
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
public class Figure_appearance {
    public string appeared_figure;
    public string start_moment;
    public string end_moment;

    public Figure_appearance() { }

    public Figure_appearance(general.IFigure_appearance appearance) {
        
        appeared_figure = appearance.figure.id;
        start_moment = appearance.start_moment.ToString();
        end_moment = appearance.end_moment.ToString();
    }

    public BigInteger get_start_moment() {
        BigInteger result;
        BigInteger.TryParse(start_moment, out result);
        return result;
    }
    public BigInteger get_end_moment() {
        BigInteger result;
        BigInteger.TryParse(end_moment, out result);
        return result;
    }
}

[Serializable]
public class Position {
    public float x;
    public float y;
    public float z;

    public Position() { }

    public Position(UnityEngine.Vector3 unity_vector) {
        x = unity_vector.x;
        y = unity_vector.y;
        z = unity_vector.z;
    }

    public UnityEngine.Vector3 to_unity() {
        return new UnityEngine.Vector3(
            x,y,z
        );
    }
}



}