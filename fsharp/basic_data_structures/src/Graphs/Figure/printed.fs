module rvinowise.ai.printed.Figure

open rvinowise
open rvinowise.ai
open rvinowise.ai.figure_parts
open System.Text
open rvinowise.extensions
open System.Diagnostics.Contracts

let private branching_edges_to_string (edges:Edge seq) =
    let result = StringBuilder()
    result 
    += "Figure( "
    edges
    |>Seq.iter(fun edge ->
        result 
        ++ edge.tail
        ++"->"
        ++ edge.head
        +=" "
    )
    result+=")"
    result.ToString()


let id_of_a_sequence_from_edges
    (edges: Edge seq) 
    (subfigures: Vertex_data) 
    =
    let first_vertex =
        edges
        |>Edges.first_vertices
        |>Seq.head
    
    let rec build_id 
        (edges : Edge seq)
        (subfigures: Vertex_data) 
        id
        (vertex:Vertex_id)
        =
        let updated_id = id+subfigures[vertex]
        vertex
        |>Edges.next_vertices edges
        |>Seq.tryHead
        |>function
        |None->updated_id
        |Some next_vertex ->
            build_id
                edges
                subfigures
                updated_id
                next_vertex
    build_id edges subfigures "" first_vertex

let private sequential_edges_to_string 
    (edges:Edge seq)
    (subfigures: Vertex_data) 
    =
    id_of_a_sequence_from_edges edges subfigures

let private edges_to_string 
    (edges:Edge seq)
    (subfigures: Vertex_data) 
    =
    if Edges.is_sequence edges then
        sequential_edges_to_string edges subfigures
    else 
        branching_edges_to_string edges

let private signal_to_string (subfigures:Vertex_data) =
    Contract.Assume(Seq.length subfigures = 1)
    let signal =
        subfigures
        |>Seq.head
    $"Signal({signal.Key}={signal.Value})"

let to_string edges subfigures  =
    if (Seq.isEmpty edges) then
        subfigures
        |>signal_to_string     
    else
        edges_to_string edges subfigures







