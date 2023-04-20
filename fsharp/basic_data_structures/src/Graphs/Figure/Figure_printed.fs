module rvinowise.ai.printed.Figure

open rvinowise
open rvinowise.ai
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
        ++ (edge.tail|>Vertex_id.value)
        ++"->"
        ++ (edge.head|>Vertex_id.value)
        +=" "
    )
    result+=")"
    result.ToString()


let id_of_a_sequence_from_edges
    edges
    (subfigures: Map<Vertex_id, Figure_id>) 
    =
    let first_vertex =
        edges
        |>Edges.first_vertices
        |>Seq.head
    
    let rec build_id 
        edges
        (subfigures: Map<Vertex_id, Figure_id>) 
        id
        (vertex:Vertex_id)
        =
        let updated_id = id+ subfigures[vertex]
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
    build_id edges subfigures (Figure_id "") first_vertex

let private sequential_edges_to_string 
    edges
    (subfigures) 
    =
    id_of_a_sequence_from_edges edges subfigures
    |>Figure_id.value

let private edges_to_string 
    edges
    (subfigures) 
    =
    if Edges.is_sequence edges then
        sequential_edges_to_string edges subfigures
    else 
        branching_edges_to_string edges

let private signal_to_string (subfigures:Map<Vertex_id, Figure_id>) =
    Contract.Assume(Seq.length subfigures = 1)
    subfigures
    |>Map.values
    |>Seq.head
    |>Figure_id.value

let to_string edges subfigures  =
    if (Seq.isEmpty edges) then
        subfigures
        |>signal_to_string     
    else
        edges_to_string edges subfigures







