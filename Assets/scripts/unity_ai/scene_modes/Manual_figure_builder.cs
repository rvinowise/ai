
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using rvinowise.ai.general;
using rvinowise.rvi.contracts;
using rvinowise.unity.extensions;
using rvinowise.unity.extensions.attributes;
using rvinowise.unity.ui.input;
using rvinowise.unity.ui.input.mouse;
using UnityEngine.EventSystems;
using rvinowise.unity.geometry2d;
using Vector3 = UnityEngine.Vector3;
using Input = UnityEngine.Input;

namespace rvinowise.ai.unity {

public class Manual_figure_builder: 
MonoBehaviour,
IFigure_button_click_receiver,
ISubfigure_click_receiver 
{
    #region members given from outside
    [SerializeField] private bool change_connections;
    
    [SerializeField] public Figure_observer figure_observer;
    [SerializeField] public Mode_selector mode_selector;

    [SerializeField] private Figure_showcase figure_showcase;
    private IFigure_provider figure_provider;

    #endregion members given from outside
    
    private Figure built_figure;
    private Figure_header figure_header;
    private Figure_representation built_repr;

    private readonly HashSet<Subfigure> selected_subfigures = new HashSet<Subfigure>();
    private readonly HashSet<Connection> selected_connections = new HashSet<Connection>();

    public void init(
        IFigure_provider figure_provider
    ) {
        this.figure_provider = figure_provider;
    }
    
    public void on_create_empty_figure() {
        built_figure = figure_provider.create_figure("f") as Figure;
        on_start_editing_figure(built_figure, true);
        built_repr = built_figure.create_representation() as Figure_representation;
        figure_header.mode_selector = mode_selector;
        figure_observer.observe(built_figure);
    }

    public void on_start_editing_figure(
        Figure figure, bool change_connections = false
    ) {
        this.change_connections = change_connections;
        activate();
        built_figure = figure;
        built_repr = figure.get_representations().FirstOrDefault() as Figure_representation;
        figure_header = built_figure.header;
        figure_header.start_building();
        figure_observer.observe(built_figure);
        if (built_repr) {
            connect_subfigures_to_builder(built_figure);
        }
    }

    private void connect_subfigures_to_builder(IFigure figure) {
        foreach(
            Subfigure subfigure in 
            figure.get_representations().First().get_subfigures()
        ) {
            connect_subnode_to_this_builder(subfigure);
        }
    }
    private void disconnect_subfigures_from_builder(IFigure figure) {
        foreach(
            Subfigure subfigure in 
            figure.get_representations().First().get_subfigures()
        ) {
            disconnect_subnode_from_this_builder(subfigure);
        }
    }
    private void connect_subnode_to_this_builder(Subfigure subfigure) {
        subfigure.manual_figure_builder = this;
        subfigure.click_receiver = this;
    }
    private void disconnect_subnode_from_this_builder(Subfigure subfigure) {
        subfigure.manual_figure_builder = null;
        subfigure.click_receiver = null;
    }
    
    private void activate() {
        enabled = true;
        figure_showcase.receiver = this;
        
    }
    public void deactivate() {
        enabled = false;
        if (built_figure) {
            figure_header.finish_building();
            disconnect_subfigures_from_builder(built_figure);
        }
    }

    void Update() {
        if (built_repr == null) {
            return;
        }
        if (UnityEngine.Input.GetMouseButtonDown(0)) {
            if (is_clicked_on_emptyness()) {
                deselect_all();
                
            }
            if (change_connections) {
                check_clicking_on_connection();
            }
        }

        update_moving_selected_subfigures();
        check_keyboard_commands();
    }

    private bool is_clicked_on_emptyness() {
        return Unity_input.instance.get_object_under_mouse() == null;
    }



    const float connection_width = 0.1f;
    private void check_clicking_on_connection() {
        foreach(Subfigure origin_subfigure in built_repr.get_subfigures()) {
            foreach(Subfigure next_subfigure in origin_subfigure.next) {
                Vector3 closest_point;
                float distance = Distance_from_point_to_line.get_distance(
                    Unity_input.instance.mouse_world_position,
                    origin_subfigure.transform.position,
                    next_subfigure.transform.position,
                    out closest_point
                );
                
                if (distance < connection_width) {
                    select(
                        origin_subfigure.get_connection_to_next(next_subfigure)
                    );
                }
            }
        }
    }

    public void on_click(Figure_button figure_button) {
        deselect_all();
        create_subfigure(figure_button.figure);
    }
    
    public void on_click_stencil_interface(Stencil_interface direction) {
        deselect_all();
        create_node_for_stencil_interface(direction);
    }

    private void create_subfigure(Figure figure) {
        Subfigure subfigure = 
            built_repr.create_subfigure(figure) as Subfigure;
        connect_subnode_to_this_builder(subfigure);
    }
    
    private void create_node_for_stencil_interface(Stencil_interface direction) {
        Subfigure subfigure = 
            built_repr.create_subfigure(direction) as Subfigure;
        connect_subnode_to_this_builder(subfigure);
    }

    
    
    public void on_click(Subfigure subfigure) {
        deselect_all();
        select(subfigure);
        Mover_of_selected.instance.add_moved_thing(subfigure);
    }

    private void update_moving_selected_subfigures() {
        //if (selected_subfigures.Any()) {
            Mover_of_selected.instance.update();
            if (Mover_of_selected.instance.moved_in_this_frame) {
                update_direction_of_connections();
            }
        //}
    }

    private void check_keyboard_commands() {
        if (change_connections) {
            if (UnityEngine.Input.GetButton("delete")) {
                foreach (Connection connection in selected_connections) {
                    connection.delete();
                }
                foreach (Subfigure subfigure in selected_subfigures) {
                    built_repr.delete_subfigure(subfigure);
                }
                deselect_all();
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
        Mover_of_selected.instance.remove_all_moved_things();
    }

    private void select(Subfigure subfigure) {
        mark_object_as_selected(subfigure);
        selected_subfigures.Add(subfigure);
    }
    private void select(Connection connection) {
        mark_object_as_selected(connection);
        selected_connections.Add(connection);
    }


    public void on_subfigures_touched(Subfigure moved_subfigure, Subfigure other_subfigure) {
        if (!change_connections) {
            return;
        }
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
        foreach (Subfigure next_subfigure in subfigure.next.ToArray() ) {
            if (
                subfigure.transform.position.x > 
                next_subfigure.transform.position.x
            ) {
                subfigure.disconnect_from_next(next_subfigure);
                next_subfigure.connext_to_next(subfigure);
                Debug.Log("next_subfigure changes to prev");
            }
        }
        foreach (Subfigure prev_subfigure in subfigure.previous.ToArray() ) {
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