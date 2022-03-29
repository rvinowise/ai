using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using rvinowise;
using rvinowise.rvi.contracts;
using rvinowise.ai.unity;
using Action = rvinowise.ai.unity.Action;
using rvinowise.ai.general;
using rvinowise.unity.geometry2d;
using UnityEngine.Assertions;
using rvinowise.unity.ui.input.mouse;
using rvinowise.ai.unity;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Selectable = rvinowise.unity.ui.input.mouse.Selectable;

namespace rvinowise.unity.ui.input {

public class Selector : MonoBehaviour {

    #region selected elements

    public Action_history action_history;
    public IFigure_provider figure_provider;
    [SerializeField] private Toggle toggle_pleasure;
    [SerializeField] private Toggle toggle_pain;
    
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
    public HashSet<ISelectable> highlighted_objects = new HashSet<ISelectable>();

    #endregion selected elements
    
    public static Selector instance;

    public Color selected_color = new Color(0,1,0);
    public Color normal_color = new Color(1,1,1);
    public Color highlighted_color = new Color(0.9f,1,0.9f);
    public Mover_of_selected mover_of_selected;
    private Vector3 get_mouse_position_from_top() {
        return new Vector3(
            rvinowise.unity.ui.input.Unity_input.instance.mouse_world_position.x,
            rvinowise.unity.ui.input.Unity_input.instance.mouse_world_position.y,
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
        if (!instance.selected(selectable)) {
            selectable.accept_selection(instance);
        }
    }
    public static void deselect(ISelectable selectable) {
        if (instance.selected(selectable)) {
            selectable.accept_deselection(instance);
        }
        
    }


    public void select(Selectable selectable) {
        select_generally(selectable);
    }
    public void deselect(Selectable selectable) {
        deselect_generally(selectable);
    }
    public void select(Figure figure) {
        select_generally(figure);
        show_insides_of_one_figure(figure);
        figure.button?.highlight_as_selected();
        figures.Add(figure);
        foreach (Figure_appearance appearance in figure.get_appearances()) {
            highlight(appearance);
        }
    }

    private void show_insides_of_one_figure(Figure shown_figure) {
        shown_figure.show_inside();
        foreach (Figure figure in figures) {
            if (shown_figure != figure) {
                figure.hide_inside();
            }
        }
    }
    
    

    
    public void deselect(Figure figure) {
        deselect_generally(figure);
        figure.hide_inside();
        figure.button?.dehighlight_as_selected();
        figures.Remove(figure);
        foreach (Figure_appearance appearance in figure._appearances) {
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
    public void dehighlight(Figure figure) {
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
    public void highlight(Figure_appearance appearance) {
        highlight_generally(appearance);
        highlighted_objects.Add(appearance);
        appearance.bezier.gameObject.SetActive(true);
        highlight(appearance.appearance_start);
        highlight(appearance.appearance_end);
    }
    public void dehighlight(Figure_appearance appearance) {
        dehighlight_generally(appearance);
        highlighted_objects.Remove(appearance);
        appearance.bezier.gameObject.SetActive(false);
        dehighlight(appearance.appearance_start);
        dehighlight(appearance.appearance_end);
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
        highlight(action.figure_appearance_impl.figure as Figure);
    }

    public void deselect(Action action) {
        deselect_generally(action);
        actions.Remove(action);
        dehighlight(action.figure_appearance_impl.figure as Figure);
    }
    public void highlight(Action action) {
        highlight_generally(action);
        highlighted_objects.Add(action);
    }

    public void dehighlight(Action action) {
        dehighlight_generally(action);
        highlighted_objects.Remove(action);
    }
    
    private void select_generally(ISelectable selectable) {
        selectables.Add(selectable);
        mark_object_as_selected(selectable);
    }
    private void deselect_generally(ISelectable selectable) {
        selectables.Remove(selectable);
        mark_object_as_deselected(selectable);
    }

    private void highlight_generally(ISelectable highlightable) {
        if (selected(highlightable)) {
            return;
        }
        if (highlightable.selection_sprite_renderer!=null) {
            highlightable.selection_sprite_renderer.material.color = highlighted_color;
        }
    }
    private void dehighlight_generally(ISelectable highlightable) {
        if (selected(highlightable)) {
            return;
        }
        if (highlightable.selection_sprite_renderer!=null) {
            highlightable.selection_sprite_renderer.material.color = normal_color;
        }
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
    
    public bool selected(ISelectable selectable) {
        return selectables.Contains(selectable);
    }
    
    private void mark_object_as_selected(ISelectable selectable) {
        if (selectable.selection_sprite_renderer!=null) {
            selectable.selection_sprite_renderer.material.color = selected_color;
        }
    }
    private void mark_object_as_deselected(ISelectable selectable) {
        if (selectable.selection_sprite_renderer!=null) {
            selectable.selection_sprite_renderer.material.color = normal_color;
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
    void Update_() {
        
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