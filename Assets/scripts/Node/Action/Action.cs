using rvinowise.ai.general;
using rvinowise.ai.ui.general;
using rvinowise.ai.ui.unity;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;
using TMPro;
using UnityEngine;


namespace rvinowise.ai.unity {


public class Action: 
MonoBehaviour,
IVisual_action
{
    #region IAction

    public Action_type type => _type;
    [SerializeField] private Action_type _type;
    public IFigure figure => figure_appearance.figure;
    public IFigure_appearance figure_appearance {
        get => figure_appearance_impl;
        set => figure_appearance_impl = value as Figure_appearance;
    }

    public IAction_group action_group{get;set;}

    #endregion IAction
    
    

    #region visualisation
    
    [SerializeField]
    private TextMeshPro label;
    
 
    public void set_label(string in_text) {
        label.text = in_text;
    }
   
    #region IHave_destructor
    
    public void destroy() {
        action_group.remove_action(this);
        this.destroy_object();
    }
    #endregion

    #region IAccept_selection

    private void Awake() {
        selection_of_rendered = new Selection_of_rendered_object(
            GetComponent<SpriteRenderer>()    
        );
    }
    public ai.ui.general.Selection_of_object selection => selection_of_rendered;

    private Selection_of_rendered_object selection_of_rendered;
    
    #endregion IAccept_selection
    
    #region debug

    [SerializeField] public Figure_appearance figure_appearance_impl;

    #endregion debug

    #endregion visualisation
}
}