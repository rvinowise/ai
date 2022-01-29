
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
    [SerializeField] private Transform cursor;

    private HashSet<Subfigure> selected_subfigures = new HashSet<Subfigure>();
    private HashSet<Connection> selected_connections = new HashSet<Connection>();
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
            deselect_all();
            check_clicking_on_object();
            check_clicking_on_connection();
        }

        process_moving_selected_subfigures();
    }

    private void check_clicking_on_object() {
        Transform clicked_object = Unity_input.instance.get_object_under_mouse();
        if (clicked_object != null) {
            process_clicking_on_object(clicked_object);
        }
    }
    private void check_clicking_on_connection() {
        cursor.transform.position = Unity_input.instance.mouse_world_position;
        foreach(Subfigure origin_subfigure in built_repr.get_subfigures()) {
            foreach(Subfigure next_subfigure in origin_subfigure.next) {
                RaycastHit hit;
                Physics.Linecast(
                    origin_subfigure.transform.position,
                    next_subfigure.transform.position,
                    out hit
                );
                if (hit.transform == cursor) {
                    select(origin_subfigure);
                }
            }
        }
    }
    private void process_clicking_on_object(Transform thing) {
        if (
            thing.GetComponent<Figure_button>() 
            is Figure_button figure_button
        ) {
            Subfigure subfigure = 
                built_repr.create_subfigure(figure_button.figure) as Subfigure;
            subfigure.manual_figure_builder = this;
        } else if (
            Unity_input.instance.get_component_under_mouse<Subfigure>() 
            is Subfigure subfigure
        ) {
            
            select(subfigure);
            Mover_of_selected.instance.remove_all_moved_things();
            Mover_of_selected.instance.add_moved_thing(subfigure);
        }
    }



    private void process_moving_selected_subfigures() {
        if (selected_subfigures.Any()) {
            Mover_of_selected.instance.update();
            if (Mover_of_selected.instance.moved_since_last_click) {
                update_direction_of_connections();
            }
        }
    }
    
    private void mark_object_as_selected(Subfigure selectable) {
        if (selectable.selection_sprite_renderer!=null) {
            selectable.selection_sprite_renderer.material.color = Selector.instance.selected_color;
        }
    }
    private void mark_object_as_selected(Connection connection) {
        connection.set_color(Selector.instance.selected_color);
    }
    private void mark_object_as_deselected(ISelectable selectable) {
        if (selectable.selection_sprite_renderer!=null) {
            selectable.selection_sprite_renderer.material.color = Selector.instance.normal_color;
        }
    }
    private void mark_object_as_deselected(Connection connection) {
        connection.set_color(Selector.instance.normal_color);
    }


    public void deselect_all() {
        foreach (ISelectable selectable in selected_subfigures.ToArray<ISelectable>()) {
            mark_object_as_deselected(selectable);
        }
        foreach (Connection selectable in selected_connections.ToArray<Connection>()) {
            mark_object_as_deselected(selectable);
        }
        selected_subfigures.Clear();
        selected_connections.Clear();
    }

    private void select(Subfigure subfigure) {
        mark_object_as_selected(subfigure);
        selected_subfigures.Add(subfigure);
    }
    private void select(Connection connection) {
        mark_object_as_selected(connection);
        selected_connections.Add(connection);
    }


    public void subfigures_touched(Subfigure moved_subfigure, Subfigure other_subfigure) {
        if (moved_subfigure.is_connected(other_subfigure)) {
            return;
        }
        Subfigure first_subfigure;
        Subfigure second_subfigure;
        if (moved_subfigure.transform.position.x < other_subfigure.transform.position.x) {
            first_subfigure = moved_subfigure;
            second_subfigure = other_subfigure;
        }
        else {
            first_subfigure = other_subfigure;
            second_subfigure = moved_subfigure;
        }
        first_subfigure.connext_to_next(second_subfigure);
    }

    private void update_direction_of_connections() {
        foreach (Subfigure subfigure in selected_subfigures) {
            update_direction_of_connections(subfigure);
        }
    }

    private void update_direction_of_connections(Subfigure subfigure) {
        foreach (Subfigure next_subfigure in subfigure.next ) {
            if (
                subfigure.transform.position.x > 
                next_subfigure.transform.position.x
            ) {
                subfigure.disconnect_from_next(next_subfigure);
                next_subfigure.connext_to_next(subfigure);
                Debug.Log("next_subfigure changes to prev");
            }
        }
        foreach (Subfigure prev_subfigure in subfigure.previous ) {
            if (
                subfigure.transform.position.x <
                prev_subfigure.transform.position.x
            ) {
                prev_subfigure.disconnect_from_next(subfigure);
                subfigure.connext_to_next(prev_subfigure);
                Debug.Log("prev_subfigure changes to next");
            }
        }
    }
}
}