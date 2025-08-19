namespace rvinowise.ai.built
    
open Xunit
open FsUnit

open System.Diagnostics.Contracts

open rvinowise
open rvinowise.ai
open rvinowise.extensions


exception BadGraph of string

module Figure=
    
    let is_empty (figure:Constant_figure) =
        figure.targets
        |>Map.isEmpty

    let error_because_of_cycles (figure:Constant_figure) =
        figure
        |>Figure.first_vertices
        |>List.ofSeq
        |>function
        |[]->
            if is_empty figure then
                None
            else
                Some "non-empty figure without first vertices, possibly because of a loop in them"
        |[signal]->None
        |many_vertices->
            many_vertices
            |>Seq.map (Edges.vertex_which_goes_into_cycle figure.edges)
            |>Seq.tryPick id
            |>(fun cycled_vertex ->
                match cycled_vertex with
                |Some vertex ->
                    Some $"figure has a loop with vertex \"{vertex}\""
                |None->
                    None
            )

    
    let error_in_correspondence_between_subfigures_and_edges (figure:Constant_figure)=
        let subfigures_in_edges = 
            figure.edges
            |>Seq.collect (fun edge->
                [edge.head;edge.tail]
            )|>Set.ofSeq
        let subfigures = 
            figure.targets
            |>Map.keys
            |>Set.ofSeq
        let difference =
            subfigures_in_edges
            |>Set.difference subfigures
        if difference.IsEmpty then
            None
        else
            Some $"superfluous subfigures: {difference}"

    

    let check_correctness (figure:Constant_figure)=
        [
            error_because_of_cycles;
            error_in_correspondence_between_subfigures_and_edges;
        ]|>List.map(fun check ->
            check figure
        )|>List.choose id
        |>function
        |[]->figure
        |errors->
            errors
            |>String.concat "\n"
            |>BadGraph
            |>raise

    let simple
        figure_id_to_name
        (turn_vertex_id_into_figure_id)
        (edges:seq<string*string>) =
        {
            edges=Graph.simple edges
            targets=
                edges
                |>Seq.map (fun(tail_id,head_id)->
                    [
                        (
                            Vertex_id tail_id
                            ,  
                            tail_id
                            |>turn_vertex_id_into_figure_id
                        );
                        (
                            Vertex_id head_id
                            ,
                            head_id 
                            |>turn_vertex_id_into_figure_id
                        )
                    ]
                )
                |>Seq.concat
                |>Map.ofSeq
        }
        |>check_correctness
        |>Renaming_figures.rename_vertices_to_standard_names figure_id_to_name
    
    
    let simple_without_separator (edges:seq<string*string>) =
        simple
            Figure_registry.id_into_name
            (String.remove_number >> Figure_registry.provide_signal)
            edges

    let simple_with_separator (edges:seq<string*string>) =
        simple
            Figure_registry.id_into_name
            (String.remove_number_with_hash >> Figure_registry.provide_signal)
            edges

    let sequential_figure_from_sequence_of_figures
        (figure_name_to_id: string -> Constant_figure_id)        
        (figure_id_to_name: Constant_figure_id -> string )        
        (figures: string seq)
        =
        let subfigures_sequence = 
            figures
            |>built.Graph.unique_numbers_for_names_in_sequence
            |>Seq.map (fun (vertex, figure_name) ->
                vertex,
                figure_name
                |>figure_name_to_id
            )
        {
            edges=
                subfigures_sequence
                |>Seq.map fst
                |>built.Graph.sequential_edges
                
            targets=
                subfigures_sequence
                |>Map.ofSeq
        }|>Renaming_figures.rename_vertices_to_standard_names figure_id_to_name

    let sequential_figure_from_sequence_of_vertices
        (turn_vertex_id_into_figure_id)
        (vertices: string seq)
        =
        let vertices_sequence = 
            vertices
            |>Seq.map (fun vertex->
                Vertex_id vertex
                ,
                turn_vertex_id_into_figure_id vertex
            )
        {
            edges=
                vertices_sequence
                |>Seq.map fst
                |>built.Graph.sequential_edges
                
            targets=
                vertices_sequence
                |>Map.ofSeq
        }//|>Renaming_figures.rename_vertices_to_standard_names
    
    let sequential_figure_from_sequence_of_subfigures
        (turn_vertex_id_into_figure_id)
        (subfigures)
        =
        let vertices_sequence = 
            subfigures
            |>Seq.map (fun subfigure->
                Vertex_id subfigure
                ,
                turn_vertex_id_into_figure_id subfigure
            )
        {
            edges=
                vertices_sequence
                |>Seq.map fst
                |>built.Graph.sequential_edges
                
            targets=
                vertices_sequence
                |>Map.ofSeq
        }//|>Renaming_figures.rename_vertices_to_standard_names
    
    let sequential_figure_from_text (text:string) =
        text
        |>Seq.map string
        |>sequential_figure_from_sequence_of_figures
            Figure_registry.provide_signal
            Figure_registry.id_into_name

    [<Fact>]
    let ``try sequence_from_text``()=
        "abba"
        |>sequential_figure_from_text
        |>should equal
            {
                edges=
                    ["a#1","b#1";"b#1","b#2";"b#2","a#2"]
                    |>Seq.map Edge.ofStringPair
                    |>Set.ofSeq
                targets=
                    ["a#1","a";"a#2","a";"b#1","b";"b#2","b"]
                    |>Seq.map (fun (vertex,figure) ->
                        Vertex_id vertex
                        ,
                        Figure_registry.provide_signal figure
                    )
                    |>Map.ofSeq
            }

    let signal
        figure_name_to_id
        figure_id_to_name
        (name:string)
        =
        {
            edges=Set.empty
            targets=[
                //(id+"#1")|>Vertex_id,
                Vertex_id name
                ,
                figure_name_to_id name
            ]|>Map.ofSeq
        }|>Renaming_figures.rename_vertices_to_standard_names figure_id_to_name

    let vertex_data_from_edges_of_figure (full_vertex_data: Map<Vertex_id, Figure_id>) edges =
        edges
        |>Edges.all_vertices
        |>Seq.map (fun vertex->
            match full_vertex_data.TryFind(vertex) with
            |Some referenced_figure -> (vertex,referenced_figure)
            |None->
                invalidArg 
                    (nameof edges + " or " + nameof full_vertex_data)
                    "the taken edges of the provided figure must not have verticex, which are not in that figure"
        )
        |>Map.ofSeq
    
    let vertex_data_from_vertices_of_figure 
        (full_vertex_data: Map<Vertex_id, Constant_figure_id>) 
        (vertices: Vertex_id seq)
        =
        vertices
        |>Seq.map (fun vertex->
            vertex, full_vertex_data|>Map.find vertex
        )
        |>Map.ofSeq


    let vertex_data_from_tuples
        figure_name_to_id
        (edges:seq<string*string*string*string>) 
        =
        edges
        |>Seq.map (fun(tail_vertex,tail_target,head_vertex,head_target)->
            [
                (Vertex_id tail_vertex, figure_name_to_id tail_target);
                (Vertex_id head_vertex, figure_name_to_id head_target)
            ]
        )
        |>Seq.concat
        |>Map.ofSeq 

    let from_parts_of_figure
        (figure:Constant_figure)
        (vertices:Vertex_id seq)
        (edges:Edge seq) =
        {
            edges=edges|>Set.ofSeq
            targets=(vertex_data_from_vertices_of_figure figure.targets vertices)
        }

    let from_tuples
        figure_name_to_id
        figure_id_to_name
        (edges:seq<string*string*string*string>) =
        {
            edges=Graph.from_tuples edges
            targets=vertex_data_from_tuples figure_name_to_id edges 
        }
        |>check_correctness
        |>Renaming_figures.rename_vertices_to_standard_names figure_id_to_name

    
    let subgraph_with_vertices 
        original_figure 
        vertices
        =
        vertices
        |>Edges.edges_between_vertices original_figure.edges
        |>from_parts_of_figure original_figure vertices 
        

module Conditional_figure =
    let from_figure_without_impossibles (figure:Constant_figure) =
        {
            Conditional_figure.existing = figure
            impossibles = Set.empty 
        }