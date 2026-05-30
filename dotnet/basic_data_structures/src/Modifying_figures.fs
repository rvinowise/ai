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
        =()
        
    
    let add_edges_from_vertices
        (head_vertex: Vertex_id)
        tail_vertices
        (edges: Set<Edge>)
        =
        tail_vertices
        |>Seq.fold ( fun all_edges tail_vertex ->
            all_edges
            |>Set.add ( Edge(tail_vertex, head_vertex) )
        ) 
            edges
    let add_edges_towards_vertices
        (tail_vertex: Vertex_id)
        head_vertices
        (edges: Set<Edge>)
        =
        head_vertices
        |>Seq.fold ( fun all_edges head_vertex ->
            all_edges
            |>Set.add ( Edge(tail_vertex, head_vertex) )
        ) 
            edges
        
    let fuse_vertices_into_subfigure
        target_name_to_id
        subfigure_name
        fused_vertices
        fused_edges
        owner_figure
        =
        let new_subfigure_id =
            target_name_to_id subfigure_name
            
        //built.Figure.subgraph_with_vertices owner_figure fused_vertices
        
        let new_vertex = Vertex_id "test"
        
      
        
        let first_fused_vertices = Edges.first_vertices fused_edges
        let last_fused_vertices = Edges.last_vertices fused_edges
        
        let vertices_after_subfigure =
            Search_in_graph.first_vertices_reacheble_from_all_vertices_together
                (fun _ -> true)
                (Edges.next_vertices owner_figure.edges)
                last_fused_vertices
        
        let vertices_before_subfigure =
            Search_in_graph.first_vertices_reacheble_from_all_vertices_together
                (fun _ -> true)
                (Edges.previous_vertices owner_figure.edges)
                first_fused_vertices
        
        let updated_edges =
            owner_figure.edges
            |>add_edges_from_vertices new_vertex vertices_before_subfigure
            |>add_edges_towards_vertices new_vertex vertices_after_subfigure
        
        let updated_targets =
            owner_figure.targets
            |>Map.add new_vertex new_subfigure_id 
        
        
        let updated_owner_figure =
            {
                owner_figure with
                    targets = updated_targets
                    edges = updated_edges
            }
            
        updated_owner_figure