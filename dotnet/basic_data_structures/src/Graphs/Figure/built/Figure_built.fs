namespace rvinowise.ai.built
    
open Xunit
open FsUnit

open System.Diagnostics.Contracts

open rvinowise.ai
open rvinowise.extensions


exception BadGraph of string

module Figure=
    
    let is_empty (figure:Figure) =
        figure.subfigures
        |>Map.isEmpty

    let error_because_of_cycles (figure:Figure) =
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

    
    let error_in_correspondence_between_subfigures_and_edges (figure:Figure)=
        let subfigures_in_edges = 
            figure.edges
            |>Seq.collect (fun edge->
                [edge.head;edge.tail]
            )|>Set.ofSeq
        let subfigures = 
            figure.subfigures
            |>Map.keys
            |>Set.ofSeq
        let difference =
            subfigures_in_edges
            |>Set.difference subfigures
        if difference.IsEmpty then
            None
        else
            Some $"superfluous subfigures: {difference}"

    

    let check_correctness (figure:Figure)=
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
        (turn_vertex_id_into_figure_id: string->string)
        (edges:seq<string*string>) =
        {
            edges=Graph.simple edges
            subfigures=
                edges
                |>Seq.map (fun(tail_id,head_id)->
                    [
                        (
                            Vertex_id tail_id
                            ,  
                            tail_id
                            |>turn_vertex_id_into_figure_id
                            |>Figure_id
                            |>built.Subfigure.referencing_constant_figure
                        );
                        (
                            Vertex_id head_id
                            ,
                            head_id 
                            |>turn_vertex_id_into_figure_id
                            |>Figure_id
                            |>built.Subfigure.referencing_constant_figure

                        )
                    ]
                )
                |>Seq.concat
                |>Map.ofSeq
        }
        |>check_correctness
        |>Renaming_figures.rename_vertices_to_standard_names
    
    
    let simple_without_separator (edges:seq<string*string>) =
        simple String.remove_number edges

    let simple_with_separator (edges:seq<string*string>) =
        simple String.remove_number_with_hash edges

    let sequential_figure_from_sequence_of_figures (figures: string seq) =
        let subfigures_sequence = 
            figures
            |>built.Graph.unique_numbers_for_names_in_sequence
            |>Seq.map (fun (vertex, figure) ->
                vertex,
                figure
                |>Figure_id
                |>built.Subfigure.referencing_constant_figure
            )
        {
            edges=
                subfigures_sequence
                |>Seq.map fst
                |>built.Graph.sequential_edges
                
            subfigures=
                subfigures_sequence
                |>Map.ofSeq
        }|>Renaming_figures.rename_vertices_to_standard_names

    let sequential_figure_from_sequence_of_vertices
        (turn_vertex_id_into_figure_id: string->string)
        (vertices: string seq)
        =
        let vertices_sequence = 
            vertices
            |>Seq.map (fun vertex->
                Vertex_id vertex
                ,
                turn_vertex_id_into_figure_id vertex
                |>Figure_id
                |>built.Subfigure.referencing_constant_figure
            )
        {
            edges=
                vertices_sequence
                |>Seq.map fst
                |>built.Graph.sequential_edges
                
            subfigures=
                vertices_sequence
                |>Map.ofSeq
        }//|>Renaming_figures.rename_vertices_to_standard_names
    
    let sequential_figure_from_sequence_of_subfigures
        (turn_vertex_id_into_figure_id: string->string)
        (subfigures: Subfigure seq)
        =
        let vertices_sequence = 
            subfigures
            |>Seq.map (fun subfigure->
                Vertex_id vertex
                ,
                turn_vertex_id_into_figure_id vertex
                |>Figure_id
                |>built.Subfigure.referencing_constant_figure
            )
        {
            edges=
                vertices_sequence
                |>Seq.map fst
                |>built.Graph.sequential_edges
                
            subfigures=
                vertices_sequence
                |>Map.ofSeq
        }//|>Renaming_figures.rename_vertices_to_standard_names
    
    let sequential_figure_from_text (text:string) =
        text
        |>Seq.map string
        |>sequential_figure_from_sequence_of_figures

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
                subfigures=
                    ["a#1","a";"a#2","a";"b#1","b";"b#2","b"]
                    |>Seq.map (fun (vertex,figure) ->
                        Vertex_id vertex
                        ,
                        Figure_id figure
                        |>built.Subfigure.referencing_constant_figure
                    )
                    |>Map.ofSeq
            }

    let signal (id:string) =
        {
            edges=Set.empty
            subfigures=[
                //(id+"#1")|>Vertex_id,
                Vertex_id id
                ,
                Figure_id id
                |>built.Subfigure.referencing_constant_figure
            ]|>Map.ofSeq
        }|>Renaming_figures.rename_vertices_to_standard_names

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
        (full_vertex_data: Map<Vertex_id, Subfigure>) 
        (vertices: Vertex_id seq)
        =
        vertices
        |>Seq.map (fun vertex->
            vertex, full_vertex_data|>Map.find vertex
        )
        |>Map.ofSeq


    let vertex_data_from_tuples 
        (edges:seq<string*string*string*string>) 
        =
        edges
        |>Seq.map (fun(tail_id,tail_ref,head_id,head_ref)->
            [
                (Vertex_id tail_id, Figure_id tail_ref|>built.Subfigure.referencing_constant_figure);
                (Vertex_id head_id, Figure_id head_ref|>built.Subfigure.referencing_constant_figure)
            ]
        )
        |>Seq.concat
        |>Map.ofSeq 

    let from_parts_of_figure
        (figure:Figure)
        (vertices:Vertex_id seq)
        (edges:Edge seq) =
        {
            edges=edges|>Set.ofSeq
            subfigures=(vertex_data_from_vertices_of_figure figure.subfigures vertices)
        }

    let from_tuples 
        (edges:seq<string*string*string*string>) =
        {
            edges=Graph.from_tuples edges
            subfigures=vertex_data_from_tuples edges
        }
        |>check_correctness
        |>Renaming_figures.rename_vertices_to_standard_names

    
    let subgraph_with_vertices 
        original_figure 
        vertices
        =
        vertices
        |>Edges.edges_between_vertices original_figure.edges
        |>from_parts_of_figure original_figure vertices 
        

module Conditional_figure =
    let from_figure_without_impossibles (figure:Figure) =
        {
            Conditional_figure.existing = figure
            impossibles = Set.empty 
        }