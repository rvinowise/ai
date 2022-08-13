using UnityEngine;
using rvinowise.ai.ui.general;
using UnityEngine.UI;

namespace rvinowise.ai.unity {

public class Figure_header: 
    MonoBehaviour,
    IFigure_header
{

    public Transform representations_folder;
    public IMode_selector mode_selector;

    public Button btn_finish_building;
    
    void Awake() {
        btn_finish_building.gameObject.SetActive(false);
    }

    #region IFigure_header
    
    public void start_building() {
        btn_finish_building.gameObject.SetActive(true);
    }

    public void finish_building() {
        btn_finish_building.gameObject.SetActive(false);
    }
    
    public void on_toggle_showing_representations() {
        representations_folder.gameObject.SetActive(
            !representations_folder.gameObject.activeInHierarchy    
        );
    }


    public void on_finish_building() {
        mode_selector.on_finish_building_figure();
    }

    #endregion IFigure_header

}
}