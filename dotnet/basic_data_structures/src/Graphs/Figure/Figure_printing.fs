namespace rvinowise.ai

open rvinowise.extensions
open rvinowise.ai
open System.Text
open System.Diagnostics.Contracts

module Figure_printing =

    let private branching_edges_to_string
        (edges:Edge seq)
        =
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


    let name_of_a_sequence_from_edges
        edges
        (subfigure_names: Map<Vertex_id, string>) 
        =
        let first_vertex =
            edges
            |>Edges.first_vertices
            |>Seq.head
        
        let rec build_id 
            edges
            (subfigure_names: Map<Vertex_id, string>) 
            name_so_far
            (vertex:Vertex_id)
            =
            let updated_id = name_so_far+ subfigure_names[vertex]
            vertex
            |>Edges.next_vertices edges
            |>Seq.tryHead
            |>function
            |None->updated_id
            |Some next_vertex ->
                build_id
                    edges
                    subfigure_names
                    updated_id
                    next_vertex
        build_id edges subfigure_names "" first_vertex


    let private edges_to_string 
        edges
        subfigures
        =
        if Edges.is_sequence edges then
            name_of_a_sequence_from_edges edges subfigures
        else 
            branching_edges_to_string edges

    let private signal_to_string (subfigure_names:Map<Vertex_id, string>) =
        Contract.Assume(Seq.length subfigure_names = 1)
        subfigure_names
        |>Map.values
        |>Seq.head

    let figure_to_string edges subfigure_names   =
        if (Seq.isEmpty edges) then
            subfigure_names
            |>signal_to_string     
        else
            edges_to_string edges subfigure_names
