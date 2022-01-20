using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using rvinowise;
using rvinowise.rvi.contracts;
using Input = rvinowise.unity.ui.input.Input;
using rvinowise.ai.unity;
using Action = rvinowise.ai.unity.Action;
using rvinowise.ai.general;
using rvinowise.unity.geometry2d;
using UnityEngine.Assertions;
using rvinowise.unity.ui.input.mouse;
using rvinowise.ai.unity;
using UnityEngine.EventSystems;

namespace rvinowise.unity.ui.input {

public class Selector : MonoBehaviour {

    #region selected elements

    public Action_history action_history;
    public Figure_storage figure_storage;
    
    public HashSet<IAction_group> action_groups = new HashSet<IAction_group>();
    public HashSet<IAction> actions = new HashSet<IAction>();
    public IReadOnlyList<IAction_group> sorted_action_groups {
        get {
            return action_groups.OrderBy(
                group => group.moment  
            ).ToList();
        }
    }
    public HashSet<IFigure> figures = new HashSet<IFigure>();
    public HashSet<Figure_appearance> figure_appearances = new HashSet<Figure_appearance>();
    public int mood;
    
    public HashSet<Subfigure> subfigures = new HashSet<Subfigure>();
    public HashSet<ISelectable> selectables = new HashSet<ISelectable>();

    #endregion selected elements
    
    public static Selector instance;

    public Color selected_color = new Color(1,0,0);
    public Color normal_color = new Color(1,1,1);
    public Mover_of_selected mover_of_selected;
    private Vector3 get_mouse_position_from_top() {
        return new Vector3(
            rvinowise.unity.ui.input.Input.instance.mouse_world_position.x,
            rvinowise.unity.ui.input.Input.instance.mouse_world_position.y,
            -100
        );
    }

    void Awake() {
        Contract.Assert(instance == null, "singleton");
        instance = this;
    }

    public int get_selected_mood() {
        return mood;
    }

    public static void select(ISelectable selectable) {
        selectable.accept_selection(instance);
    }
    public static void deselect(ISelectable selectable) {
        selectable.accept_deselection(instance);
    }


    public void select(Figure figure) {
        select_generally(figure);
        
        if (figure == figure_storage.pleasure_signal) {
            mood += 1;
        } else if (figure ==  figure_storage.pain_signal) {
            mood -= 1;
        }
        else {
            figures.Add(figure);
            foreach (Figure_appearance appearance in figure._appearances) {
                select(appearance);
            }
        }
    }
    public void deselect(Figure figure) {
        deselect_generally(figure);
        
        if (figure == figure_storage.pleasure_signal) {
            mood -= 1;
        } else if (figure ==  figure_storage.pain_signal) {
            mood += 1;
        }
        else {
            figures.Remove(figure);
            foreach (Figure_appearance appearance in figure._appearances) {
                deselect(appearance);
            }
        }
    }

    public void select(Figure_appearance appearance) {
        select_generally(appearance);
        figure_appearances.Add(appearance);
        appearance.bezier.gameObject.SetActive(true);
        select(appearance.appearance_start);
        select(appearance.appearance_end);
    }
    public void deselect(Figure_appearance appearance) {
        deselect_generally(appearance);
        figure_appearances.Remove(appearance);
        appearance.bezier.gameObject.SetActive(false);
        deselect(appearance.appearance_start);
        deselect(appearance.appearance_end);
    }
    
    public void select(Subfigure subfigure) {
        select_generally(subfigure);
        subfigures.Add(subfigure);
    }
    public void deselect(Subfigure subfigure) {
        deselect_generally(subfigure);
        subfigures.Remove(subfigure);
    }
    public void select(Action_group action_group) {
        select_generally(action_group);
        action_groups.Add(action_group);
    }
    public void deselect(Action_group action_group) {
        deselect_generally(action_group);
        action_groups.Remove(action_group);
    }
    
    public void select(Action action) {
        select_generally(action);
        actions.Add(action);
    }

    public void deselect(Action action) {
        deselect_generally(action);
        actions.Remove(action);
    }
    
    private void select_generally(ISelectable selectable) {
        selectables.Add(selectable);
        mark_object_as_selected(selectable);
    }
    private void deselect_generally(ISelectable selectable) {
        selectables.Remove(selectable);
        mark_object_as_deselected(selectable);
    }
    
    public bool selected(ISelectable selectable) {
        return selectables.Contains(selectable);
    }
    
    private void mark_object_as_selected(ISelectable selectable) {
        if (selectable.selection_sprite_renderer!=null) {
            selectable.selection_sprite_renderer.color = selected_color;
        }
    }
    private void mark_object_as_deselected(ISelectable selectable) {
        if (selectable.selection_sprite_renderer!=null) {
            selectable.selection_sprite_renderer.color = normal_color;
        }
    }


    public void deselect_all() {
        foreach(ISelectable selectable in selectables.ToArray<ISelectable>()) {
            deselect(selectable);
        }
        action_groups.Clear();
        actions.Clear();
        figures.Clear();
        subfigures.Clear();
        figure_appearances.Clear();
        selectables.Clear();
    }

    public void deselect_all_figures() {
        foreach(Figure figure in figures.ToArray<IFigure>()) {
            deselect(figure);
        }
        figures.Clear();
    }


    private IReadOnlyList<IAction_group> get_all_action_groups() {
        return action_history.get_action_groups(
            0,
            action_history.last_moment
        );// as IReadOnlyList<Action_group>;
    }

    public ISelectable last_click_target;
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
                (!last_click_selected)&&
                (!mover_of_selected.moved_since_last_click)
            ) {
                Debug.Log("release, last_click_selected==true for "+last_click_target);
                ISelectable target_of_release = get_selectable_under_mouse();
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
            ISelectable target_of_release, ISelectable target_of_last_click
        ) {
            if (target_of_release == null) {
                return false;
            }
            return target_of_release == target_of_last_click;
        } 
    }

    public ISelectable get_selectable_under_mouse() {
        Ray ray = new Ray(get_mouse_position_from_top(), Vector3.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) {
            if(
                hit.transform.GetComponent<ISelectable>() 
                is ISelectable selectable
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
            IFigure figure = figure_storage.find_figure_with_id(id);
            if (figure != null) {
                select((Figure)figure);
            } else {
                Debug.Log($"trying to select non-existing figure \"{name}\"");
            }
        }
    }
}
}