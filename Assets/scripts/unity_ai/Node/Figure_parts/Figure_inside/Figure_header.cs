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

namespace rvinowise.ai.unity {

public class Figure_header: 
MonoBehaviour
{

    //public List<Figure_representation> representations = new List<Figure_representation>();\
    public Transform representations_folder;


    public void on_toggle_showing_representations() {
        representations_folder.gameObject.SetActive(
            !representations_folder.gameObject.activeInHierarchy    
        );
    }
    public void on_show_representations() {
        representations_folder.gameObject.SetActive(true);
    }
    public void on_hide_representations() {
        representations_folder.gameObject.SetActive(false);
    }
    
    #region ISelectable

    public Collider collider { get; }
    public Renderer selection_sprite_renderer { get; }
    

    #endregion

}
}