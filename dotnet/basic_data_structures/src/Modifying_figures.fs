namespace rvinowise.ai

open System.Collections.Generic
open rvinowise
open rvinowise.ai
open rvinowise.extensions


module Modifying_figures=

    
    let add_subfigure
        
        owner_figure
        =()
    
    let figure_from_part_of_figure
        taken_vertices
        bigger_figure
        =
        
    
    let fuse_vertices_into_subfigure
        target_name_to_id
        subfigure_name
        fused_vertices
        owner_figure
        =
        let new_subfigure_id =
            target_name_to_id
            
            built.Figure.subgraph_with_vertices owner_figure fused_vertices
        
        let new_vertex = ()
        
        let first_fused_vertices = Edges.first_vertices fused_vertices
        let last_fused_vertices = Edges.last_vertices fused_vertices
        
        let vertices_after_subfigure =
            Search_in_graph.first_vertices_reacheble_from_all_vertices_together
                (fun _ -> true)
                (Edges.next_vertices owner_figure.edges)
                last_fused_vertices
        
        let vertices_before_subfigure =
            Search_in_graph.first_vertices_reacheble_from_all_vertices_together
                (fun _ -> true)
                (Edges.previous_vertices owner_figure.edges)
                last_fused_vertices
        
        owner_figure.edges
        
        owner_figure.targets
        |>Map.add new_vertex new_subfigure_id
        
        
        