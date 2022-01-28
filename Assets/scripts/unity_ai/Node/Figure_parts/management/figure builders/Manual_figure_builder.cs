
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.unity.extensions;
using rvinowise.unity.ui.input;
using rvinowise.unity.ui.input.mouse;
using UnityEngine.EventSystems;
using Vector3 = UnityEngine.Vector3;

namespace rvinowise.ai.unity {

public class Manual_figure_builder: MonoBehaviour {

    [SerializeField] private Figure_builder builder;
    private Figure_storage figure_storage => builder.figure_storage;
    private Figure figure_prefab;
    private Figure built_figure;
    private Figure_header figure_header;
    private Figure_representation built_repr;
    public Mode_selector mode_selector;

    private HashSet<Subfigure> selected_subfigures = new HashSet<Subfigure>();
    public void on_create_empty_figure() {
        built_figure = builder.create_new_figure("f") as Figure;
        figure_header = built_figure.header;
        figure_storage.append_figure(built_figure);
        built_repr = built_figure.create_representation() as Figure_representation;
        figure_header.start_building();
        figure_header.mode_selector = mode_selector;
        show_figure(built_figure);
    }

    private void show_figure(Figure figure) {
        show_insides_of_one_figure(figure);
        figure.button.highlight_as_selected();
        foreach (Figure_appearance appearance in figure.get_appearances()) {
            highlight(appearance);
        }
    }

    private void hide_figure(Figure figure) {
        figure.hide_inside();
        figure.button.dehighlight_as_selected();
        foreach (Figure_appearance appearance in figure._appearances) {
            dehighlight(appearance);
        }
    }
    public void highlight(Figure_appearance appearance) {
        appearance.bezier.gameObject.SetActive(true);
        mark_as_highlighted(appearance.appearance_start);
        mark_as_highlighted(appearance.appearance_end);
    }
    public void dehighlight(Figure_appearance appearance) {
        appearance.bezier.gameObject.SetActive(false);
        unmark_as_highlighted(appearance.appearance_start);
        unmark_as_highlighted(appearance.appearance_end);
    }
    
    private void mark_as_highlighted(ISelectable highlightable) {
        if (highlightable.selection_sprite_renderer!=null) {
            highlightable.selection_sprite_renderer.material.color = Selector.instance.highlighted_color;
        }
    }
    private void unmark_as_highlighted(ISelectable highlightable) {
        if (highlightable.selection_sprite_renderer!=null) {
            highlightable.selection_sprite_renderer.material.color = Selector.instance.normal_color;
        }
    }
    
    private void show_insides_of_one_figure(Figure shown_figure) {
        shown_figure.show_inside();
        foreach (Figure figure in figure_storage.known_figures) {
            if (shown_figure != figure) {
                figure.hide_inside();
            }
        }
    }
    
    public void activate() {
        enabled = true;
        on_create_empty_figure();
    }
    public void deactivate() {
        enabled = false;
        if (built_figure) {
            figure_header.finish_building();
            hide_figure(built_figure);
        }
    }

    void Update() {
 
        if (UnityEngine.Input.GetMouseButtonDown(0)) {
            Transform clicked_object = Unity_input.instance.get_object_under_mouse();
            if (clicked_object == null) {
                return;
            }
            if (
                clicked_object.GetComponent<Figure_button>() is Figure_button figure_button
            ) {
                Subfigure subfigure = 
                    built_repr.create_subfigure(figure_button.figure) as Subfigure;
                //Selector.select(subfigure);
                
            } else
            if (
                Unity_input.instance.get_component_under_mouse<Subfigure>() 
                    is Subfigure subfigure
            ) {
                deselect_all();
                select(subfigure);
                Mover_of_selected.instance.remove_all_moved_things();
                Mover_of_selected.instance.add_moved_thing(subfigure);
            }
        }

        Mover_of_selected.instance.update();
    }

    
    private void mark_object_as_selected(Subfigure selectable) {
        if (selectable.selection_sprite_renderer!=null) {
            selectable.selection_sprite_renderer.material.color = Selector.instance.selected_color;
        }
    }
    private void mark_object_as_deselected(ISelectable selectable) {
        if (selectable.selection_sprite_renderer!=null) {
            selectable.selection_sprite_renderer.material.color = Selector.instance.normal_color;
        }
    }


    public void deselect_all() {
        foreach (ISelectable selectable in selected_subfigures.ToArray<ISelectable>()) {
            mark_object_as_deselected(selectable);
        }
        selected_subfigures.Clear();
    }

    private void select(Subfigure subfigure) {
        mark_object_as_selected(subfigure);
        selected_subfigures.Add(subfigure);
    }
    

    private void move_subfigures(Vector3 difference) {
        if (difference.magnitude > float.Epsilon) {
            foreach(Subfigure selected_subfigure in selected_subfigures) {
                selected_subfigure.transform.position += difference;
            }
        }
        
    }

    
    
}
}