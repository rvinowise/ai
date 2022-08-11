using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using rvinowise.contracts;
using rvinowise.ai.unity;
using Action = rvinowise.ai.unity.Action;
using rvinowise.ai.general;
using rvinowise.ai.ui.general;
using rvinowise.unity.ui.input.mouse;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Network = rvinowise.ai.unity.Network;


namespace rvinowise.unity.ui.input {

//var figures = new Marked_objects().get_figures()

public class Marked_objects {

    private readonly Network network;
    private IAction_history action_history;
    private IFigure_provider<IVisual_figure> figure_provider;
    
    private Toggle toggle_pleasure;
    private Toggle toggle_pain;

    
    // public IReadOnlyList<Action_group> get_selected_action_groups() {
    //     return action_history.get_action_groups().Where(
    //         obj => obj.selected_object is Action_group    
    //     ).Select(
    //         obj => obj.selected_object as Action_group    
    //     ).OrderBy(
    //         group => group.moment  
    //     ).ToList().AsReadOnly();
    // }

    public int mood;
    
    public readonly HashSet<IAccept_selection> selected_objects = new();
    public readonly HashSet<IAccept_selection> highlighted_objects = new();
    
    public Marked_objects instance;
    
    
    public Marked_objects(Network network) {
        this.network = network;
        figure_provider = network.figure_showcase;
        action_history = network.action_history;
    }

 
    
    public Mover_of_selected mover_of_selected;
    private Vector3 get_mouse_position_from_top() {
        return new(
            rvinowise.unity.ui.input.Unity_input.instance.mouse_world_position.x,
            rvinowise.unity.ui.input.Unity_input.instance.mouse_world_position.y,
            -100
        );
    }
    

    public int get_selected_mood() {
        return mood;
    }



    public void select(IAccept_selection accept_selection) {
        select_generally(accept_selection);
    }
    public void deselect(IAccept_selection accept_selection) {
        deselect_generally(accept_selection);
    }
    public void select(Figure figure) {
        select_generally(figure);
        figure.button?.highlight_as_selected();
        foreach (Figure_appearance appearance in figure.get_appearances()) {
            highlight(appearance);
        }
    }

    public void deselect(Figure figure) {
        deselect_generally(figure);
        figure.hide();
        figure.button?.dehighlight_as_selected();
        figures.Remove(figure);
        foreach (IVisual_figure_appearance appearance in figure.get_appearances()) {
            dehighlight(appearance);
        }
        
    }
    
    public void highlight(Figure figure) {
        highlight_generally(figure);
        highlighted_objects.Add(figure);
        foreach (IFigure_appearance appearance in figure.get_appearances()) {
            highlight(appearance as Figure_appearance);
        }
    }

    private void dehighlight(Figure figure) {
        dehighlight_generally(figure);
        highlighted_objects.Remove(figure);
        foreach (IFigure_appearance appearance in figure.get_appearances()) {
            dehighlight(appearance as Figure_appearance);
        }
    }

    public void select(Figure_appearance appearance) {
        select_generally(appearance);
        figure_appearances.Add(appearance);
        appearance.bezier.gameObject.SetActive(true);
        select(appearance.start_action);
        select(appearance.end_action);
    }
    public void deselect(Figure_appearance appearance) {
        deselect_generally(appearance);
        figure_appearances.Remove(appearance);
        appearance.bezier.gameObject.SetActive(false);
        deselect(appearance.start_action);
        deselect(appearance.end_action);
    }

    private void highlight(Figure_appearance appearance) {
        highlight_generally(appearance);
        highlighted_objects.Add(appearance);
        appearance.bezier.gameObject.SetActive(true);
        highlight(appearance.start_action);
        highlight(appearance.end_action);
    }

    private void dehighlight(Figure_appearance appearance) {
        dehighlight_generally(appearance);
        highlighted_objects.Remove(appearance);
        appearance.bezier.gameObject.SetActive(false);
        dehighlight(appearance.start_action);
        dehighlight(appearance.end_action);
    }
    
 
    public void select(IVisual_action action) {
        select_generally(action);
        actions.Add(action);
        highlight(action.figure_appearance.figure as Figure);
    }

    public void deselect(IVisual_action action) {
        deselect_generally(action);
        actions.Remove(action);
        dehighlight(action.figure_appearance.figure as Figure);
    }

    private void highlight(Action action) {
        highlight_generally(action);
        highlighted_objects.Add(action);
    }

    private void dehighlight(Action action) {
        dehighlight_generally(action);
        highlighted_objects.Remove(action);
    }
    
    private void select_generally(IAccept_selection accept_selection) {
        selected_objects.Add(accept_selection);
        accept_selection.set_state(Selection_state.Selected);
    }
    private void deselect_generally(IAccept_selection accept_selection) {
        selected_objects.Remove(accept_selection);
        accept_selection.set_state(Selection_state.Normal);
    }

    private void highlight_generally(IAccept_selection highlightable) {
        if (selected(highlightable)) {
            return;
        }
        highlightable.set_state(Selection_state.Highlighted);
    }
    private void dehighlight_generally(IAccept_selection highlightable) {
        if (selected(highlightable)) {
            return;
        }
        highlightable.set_state(Selection_state.Normal);
    }
    
    //inspector event
    public void on_mood_changed() {
        if (toggle_pleasure.isOn) {
            mood = 1;
        } else if (toggle_pain.isOn) {
            mood = -1;
        } else {
            mood = 0;
        }

    }

    private bool selected(IAccept_selection accept_selection) {
        return selected_objects.Contains(accept_selection);
    }
    

    public void deselect_all() {
        foreach(var selectable in selected_objects.ToArray<IAccept_selection>()) {
            deselect(selectable);
        }
        selected_objects.Clear();
    }


    private IReadOnlyList<IAction_group> get_all_action_groups() {
        return action_history.get_action_groups(
            0,
            action_history.last_moment
        );// as IReadOnlyList<Action_group>;
    }

    private IAccept_selection last_click_target;
    bool last_click_selected;
    

    void Update() {
        
        if (UnityEngine.Input.GetMouseButtonDown (0)) {
            if (EventSystem.current.IsPointerOverGameObject()) {
                return;
            } 
            last_click_target = get_selectable_under_mouse();                        
            if (last_click_target != null) { // clicked on an object
                if (!selected(last_click_target)) { //the object is for selection by this click
                    select(last_click_target);
                    last_click_selected = true;
                    Debug.Log("select for "+last_click_target);
                } else { // the object is for moving (already was selected)
                    last_click_selected = false;
                    Debug.Log("click on selected "+last_click_target);
                }
                //mover_of_selected.start_moving_selected_objects();
            } else { // clicked on the emptiness
                deselect_all();
            }
        }
        //release button
        if (UnityEngine.Input.GetMouseButtonUp (0)) {
            if (
                !last_click_selected &&
                !mover_of_selected.moved_since_last_click
            ) {
                Debug.Log("release, last_click_selected==true for "+last_click_target);
                IAccept_selection target_of_release = get_selectable_under_mouse();
                //deselect on release. initial click is used to move objects.
                if (same_object_received_click_and_release(
                    target_of_release, last_click_target
                )) {
                    deselect(target_of_release);
                    Debug.Log("deselect for "+last_click_target);
                }
            }
        } 

        mover_of_selected.update();

        bool same_object_received_click_and_release(
            IAccept_selection target_of_release, IAccept_selection target_of_last_click
        ) {
            if (target_of_release == null) {
                return false;
            }
            return target_of_release == target_of_last_click;
        } 
    }

    public IAccept_selection get_selectable_under_mouse() {
        Ray ray = new(get_mouse_position_from_top(), Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            if(
                hit.transform.GetComponent<IAccept_selection>() 
                is IAccept_selection selectable
            ) { 
                return selectable;
            }
        }
        return null;
    }
    
    public void select_figures_from_string(string in_string) {
        deselect_all_figures();
        string[] ids = in_string.Split(' ')
            .Where(x => !string.IsNullOrWhiteSpace(x)).ToArray();
        foreach (string id in ids) {
            IFigure figure = figure_provider.find_figure_with_id(id);
            if (figure != null) {
                select((Figure)figure);
            } else {
                Debug.Log($"trying to select non-existing figure \"{id}\"");
            }
        }
    }
}
}