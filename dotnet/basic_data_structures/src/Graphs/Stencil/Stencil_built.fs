namespace rvinowise.ai.built

open rvinowise.ai
open rvinowise
open rvinowise.extensions

module Stencil =

    
    

    let remove_vertex_from_graph
        removed_vertex
        edges
        =
        let vertices_before_output =
            removed_vertex
            |>Edges.previous_vertices edges
        
        let vertices_after_output =
            removed_vertex
            |>Edges.next_vertices edges
        
        let edges_instead_of_output =
            (vertices_before_output,vertices_after_output)
            ||>Seq.allPairs
            |>Seq.map Edge.ofPair

        let edges_without_output = 
            edges
            |>Seq.filter(fun edge->
                (edge.head=removed_vertex 
                || 
                edge.tail=removed_vertex)
                |>not
            )

        edges_instead_of_output
        |>Seq.append edges_without_output
        |>Set.ofSeq

    let simple 
        (turn_vertex_id_into_figure_id: string->string)
        (raw_edges:seq<string*string>) 
        =
        let edges =
            raw_edges
            |>built.Graph.simple
            
        let vertices_before_out =
            rvinowise.ai.Stencil.output_vertex
            |>Edges.previous_vertices edges
        
        let vertices_after_out =
            Stencil.output_vertex
            |>Edges.next_vertices edges
            
        let edges_without_out =
            edges
            |>remove_vertex_from_graph Stencil.output_vertex
            
        let figure = {
            edges = edges_without_out
                
            subfigures=  
                edges_without_out
                |>Seq.map (fun edge->
                    [
                        (
                            edge.tail, 
                            edge.tail
                            |>Vertex_id.value
                            |>turn_vertex_id_into_figure_id
                            |>Figure_id
                        );
                        (
                            edge.head,
                            edge.head
                            |>Vertex_id.value
                            |>turn_vertex_id_into_figure_id
                            |>Figure_id
                        );
                    ]
                )
                |>Seq.concat
                |>Map.ofSeq
        }
            
        {
            figure=figure
            vertices_before_out = vertices_before_out
            vertices_after_out = vertices_after_out
            output_without=Set.empty
            blocking_vertices = Map.empty 
        }

    let simple_without_separator (edges:seq<string*string>) =
        simple String.remove_number edges

    let simple_with_separator (edges:seq<string*string>) =
        simple String.remove_number_with_hash edges


    let sequential_stencil_from_sequence (references_of_vertices: string seq) =
        let edges =
            references_of_vertices
            |>Seq.map Vertex_id
            |>built.Graph.sequential_edges
            
        let vertices_before_out =
            ai.Stencil.output_vertex
            |>Edges.previous_vertices edges
        
        let vertices_after_out =
            ai.Stencil.output_vertex
            |>Edges.next_vertices edges
            
        let edges_without_out =
            edges
            |>remove_vertex_from_graph ai.Stencil.output_vertex
        
        let figure = {
            edges = edges_without_out
            subfigures =
                references_of_vertices
                |>Seq.filter (fun reference -> reference <> Vertex_id.value ai.Stencil.output_vertex)
                |>built.Figure.sequence_to_vertices_and_figures
                |>Map.ofSeq
        }
       
        {
            figure=figure
                
            vertices_before_out = vertices_before_out
            vertices_after_out = vertices_before_out
            
            output_without=Set.empty
            blocking_vertices = Map.empty 
        }
