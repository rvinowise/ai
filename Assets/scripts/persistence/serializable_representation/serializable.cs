using System;
using System.Collections.Generic;
using UnityEngine;

namespace rvinowise.unity.ai.persistence.serializable {

[Serializable]
public class Action_group {
    public string guid;
    public int moment;
    public List<string> actions;
    
    #region visualisation
    public float[] position = new float[3];
    #endregion
}

[Serializable]
public class Action {
    public string guid;
    public int moment;
    public bool is_end;
    public string action_group;
}

[Serializable]
public class Figure {
    public string guid;
    public string id;
    public List<string> subfigures;
    
    #region visualisation
    public float[] position = new float[3];
    #endregion
}

[Serializable]
public class Subfigure {
    public string guid;
    public string parent_figure;
    public string referenced_figure;
    public List<string> previous_subfigures;
    public List<string> next_subfigures;
    
    #region visualisation
    public float[] position = new float[3];
    #endregion
}
[Serializable]
public class Pattern {
    public string guid;
    public string id;
    public List<string> subfigures; //actual figures and patterns
}
[Serializable]
public class Figure_appearance {
    public string guid;
    public string appeared_figure;
    public string start_action;
    public string end_action;
}

}