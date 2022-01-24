using System.Collections.Generic;
using System.Numerics;
using rvinowise.ai.general;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;
using System.Linq;
using rvinowise.ai.unity.visuals;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.ui.input.mouse;

namespace rvinowise.ai.unity {

public class Figure_representation:
MonoBehaviour,
IFigure_representation
{
    #region IFigure_representation
    public string id{get;set;}
    public IReadOnlyList<ISubfigure> get_subfigures() => subfigures.AsReadOnly();
    public IReadOnlyList<ISubfigure> get_first_subfigures() => first_subfigures.AsReadOnly();
    public void add_first_subfigures(ISubfigure subfigure) => first_subfigures.Add(subfigure);
    #endregion IFigure_representation
    public List<ISubfigure> first_subfigures = new List<ISubfigure>();

    public List<ISubfigure> subfigures = new List<ISubfigure>();
    
    
    #region building
    public ISubfigure add_subfigure(IFigure child_figure) {
        Subfigure subfigure = subfigure_prefab.
            create_for_figure(child_figure);
        subfigure.transform.parent = subfigures_folder.transform;
        subfigure.parent = this;
        position_subfigure(subfigure);
        subfigures.Add(subfigure);
        return subfigure;
    }
    private void position_subfigure(Subfigure subfigure) {
        
        subfigure.transform.position = 
            get_position_of_last_subfigure()+ subfigures_offset;
    }

    private Vector3 get_position_of_last_subfigure() {
        if (subfigures.Any()) {
            var last_subfigue = subfigures.Last() as Subfigure;
            return last_subfigue.transform.position;
        }
        return transform.position + new Vector3(0,-2,0);
    }
    
    #endregion building

    #region visualisation
    [SerializeField] public Subfigure subfigure_prefab;
    [SerializeField] private Transform subfigures_folder;
    private Vector3 subfigures_offset = new Vector3(2,0,0);
    public void show() {
        gameObject.SetActive(true);
    }

    public void toggle_showing_graph() {
        subfigures_folder.gameObject.SetActive(subfigures_folder.gameObject.activeInHierarchy);
    }
    #endregion visualisation
}
}